using Microsoft.EntityFrameworkCore;
using SalonWebApp.Models;
using System.Security.Cryptography;
using System.Text;

public static class DatabaseInitializer
{
    public static void Seed(SalonContext context)
    {
        context.Database.Migrate();

        if (!context.Users.Any(u => u.Email == "b221210083@sakarya.edu.tr"))
        {
            string hashedPassword = HashPassword("sausausau");

            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "b221210083@sakarya.edu.tr",
                Password = hashedPassword,
                PhoneNumber = "1234567890",
                Gender = Gender.MALE,
                Role = Roles.ADMIN
            };

            context.Users.Add(adminUser);
        }

        // 10 salon ekle
        if (!context.Salons.Any())
        {
            for (int i = 1; i <= 10; i++)
            {
                context.Salons.Add(new Salon
                {
                    Name = $"Salon {i}",
                    Type = SalonType.Barber,
                    OpenDays = "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday",
                    OpeningHour = new TimeSpan(9, 0, 0),
                    ClosingHour = new TimeSpan(20, 0, 0),
                    Address = $"Adres {i}",
                    Phone = $"12345678{i}",
                    Employees = new List<Employee>(),
                    Services = new List<Service>(),
                    Appointments = new List<Appointment>()
                });
            }
        }

        // 20 çalışan ekle ve her 2 çalışan bir salona ait olsun
        if (!context.Employees.Any())
        {
            // Önce salonların var olduğundan emin olun
            if (!context.Salons.Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    context.Salons.Add(new Salon
                    {
                        Name = $"Salon {i}",
                        Type = SalonType.Barber,
                        OpenDays = "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday",
                        OpeningHour = new TimeSpan(9, 0, 0),
                        ClosingHour = new TimeSpan(20, 0, 0),
                        Address = $"Adres {i}",
                        Phone = $"12345678{i}",
                        Employees = new List<Employee>(),
                        Services = new List<Service>(),
                        Appointments = new List<Appointment>()
                    });
                }
                context.SaveChanges(); // Salonları ekledikten sonra kaydedin
            }

            // Salonları listeleyin
            var salons = context.Salons.ToList();

            // Çalışanları ekleyin
            for (int i = 1; i <= 20; i++)
            {
                var salon = salons[(i - 1) / 2]; // Her 2 çalışan bir salon
                context.Employees.Add(new Employee
                {
                    FirstName = $"Çalışan {i}",
                    LastName = $"Soyad {i}",
                    Salary = 5000 + i * 100,
                    Phone = $"12345678{i}",
                    SalonId = salon.SalonId,
                    EmployeeServices = new List<EmployeeService>(),
                    WorkingDays = new List<WorkingDay>(),
                    Appointments = new List<Appointment>()
                });
            }

            context.SaveChanges(); // Çalışanları ekledikten sonra kaydedin
        }


        // 10 servis ekle
        if (!context.Services.Any())
        {
            var salons = context.Salons.ToList();
            for (int i = 1; i <= 10; i++)
            {
                context.Services.Add(new Service
                {
                    Name = $"Servis {i}",
                    Price = 50 + i * 10,
                    Duration = new TimeSpan(0, 30, 0),
                    SalonId = salons[i % salons.Count].SalonId, // Servisleri salonlara rastgele dağıt
                    EmployeeServices = new List<EmployeeService>()
                });
            }
        }

        // Her çalışana 1 veya 2 servis atama
        if (!context.EmployeeServices.Any())
        {
            var employees = context.Employees.ToList();
            var services = context.Services.ToList();
            var random = new Random();

            foreach (var employee in employees)
            {
                int serviceCount = random.Next(1, 3); // 1 veya 2 servis
                var assignedServices = services.OrderBy(x => random.Next()).Take(serviceCount).ToList();
                foreach (var service in assignedServices)
                {
                    context.EmployeeServices.Add(new EmployeeService
                    {
                        EmployeeId = employee.EmployeeId,
                        ServiceId = service.ServiceId
                    });
                }
            }
        }

        // 1 saat arayla tüm saat dilimlerini ekle
        if (!context.Times.Any())
        {
            for (int i = 9; i <= 19; i++) // 09:00 - 20:00 saatleri
            {
                context.Times.Add(new Time
                {
                    Date = DateTime.Now.Date,
                    StartTime = new TimeSpan(i, 0, 0),
                    EndTime = new TimeSpan(i + 1, 0, 0),
                    Selectable = true,
                    Appointments = new List<Appointment>()
                });
            }
        }

        // Çalışma günleri ekle ve çalışanlara ata
        if (!context.WorkingDays.Any())
        {
            var employees = context.Employees.ToList();
            foreach (var employee in employees)
            {
                for (int i = 0; i < 5; i++) // Haftanın 5 günü çalışma
                {
                    context.WorkingDays.Add(new WorkingDay
                    {
                        EmployeeId = employee.EmployeeId,
                        Date = DateTime.Now.Date.AddDays(i),
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(17, 0, 0)
                    });
                }
            }
        }

        context.SaveChanges();
    }

    private static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
