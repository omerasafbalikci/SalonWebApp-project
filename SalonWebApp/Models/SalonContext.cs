using Microsoft.EntityFrameworkCore;

namespace SalonWebApp.Models
{
    public class SalonContext : DbContext
    {
        public SalonContext(DbContextOptions<SalonContext> options) : base(options)
        {
        }

        // DbSet Properties
        public DbSet<User> Users { get; set; }
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WorkingDay> WorkingDays { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Time> Times { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.FirstName).IsRequired();
                entity.Property(u => u.LastName).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PhoneNumber).IsRequired();
                entity.Property(u => u.Gender).HasConversion<string>().IsRequired();
                entity.Property(u => u.Role).IsRequired();

                entity.HasMany(u => u.Appointments).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            });

            modelBuilder.Entity<Salon>(entity =>
            {
                entity.HasKey(s => s.SalonId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.OpenDays).IsRequired();
                entity.Property(s => s.OpeningHour).IsRequired();
                entity.Property(s => s.ClosingHour).IsRequired();
                entity.Property(s => s.Type).HasConversion<string>().IsRequired();
                entity.Property(s => s.Address).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Phone).IsRequired();

                entity.HasMany(s => s.Employees).WithOne(e => e.Salon).HasForeignKey(e => e.SalonId);
                entity.HasMany(s => s.Services).WithOne(serv => serv.Salon).HasForeignKey(serv => serv.SalonId);
                entity.HasMany(s => s.Appointments).WithOne(a => a.Salon).HasForeignKey(a => a.SalonId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Salary).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Phone).IsRequired();
                entity.Property(e => e.SalonId).IsRequired();

                entity.HasOne(e => e.Salon).WithMany(s => s.Employees).HasForeignKey(e => e.SalonId);
                entity.HasMany(e => e.EmployeeServices).WithOne(es => es.Employee).HasForeignKey(es => es.EmployeeId);
                entity.HasMany(e => e.WorkingDays).WithOne(wd => wd.Employee).HasForeignKey(wd => wd.EmployeeId);
                entity.HasMany(e => e.Appointments).WithOne(a => a.Employee).HasForeignKey(a => a.EmployeeId);
            });

            modelBuilder.Entity<EmployeeService>(entity =>
            {
                entity.HasKey(es => es.EmployeeServiceId);
                entity.Property(es => es.EmployeeId).IsRequired();
                entity.Property(es => es.ServiceId).IsRequired();

                entity.HasOne(es => es.Employee).WithMany(e => e.EmployeeServices).HasForeignKey(es => es.EmployeeId);
                entity.HasOne(es => es.Service).WithMany().HasForeignKey(es => es.ServiceId);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(serv => serv.ServiceId);
                entity.Property(serv => serv.Name).IsRequired();
                entity.Property(serv => serv.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(serv => serv.Duration).IsRequired();
                entity.Property(serv => serv.SalonId).IsRequired();

                entity.HasOne(serv => serv.Salon).WithMany(s => s.Services).HasForeignKey(serv => serv.SalonId);
                entity.HasMany(serv => serv.EmployeeServices).WithOne(es => es.Service).HasForeignKey(es => es.ServiceId);
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.HasKey(t => t.TimeId);
                entity.Property(t => t.Date).IsRequired();
                entity.Property(t => t.StartTime).IsRequired();
                entity.Property(t => t.EndTime).IsRequired();
                entity.Property(t => t.Selectable).IsRequired();

                entity.HasMany(t => t.Appointments).WithOne(a => a.Time).HasForeignKey(a => a.TimeId);
            });

            modelBuilder.Entity<WorkingDay>(entity =>
            {
                entity.HasKey(wd => wd.WorkingDayId);
                entity.Property(wd => wd.EmployeeId).IsRequired();
                entity.Property(wd => wd.Date).IsRequired();
                entity.Property(wd => wd.StartTime).IsRequired();
                entity.Property(wd => wd.EndTime).IsRequired();

                entity.HasOne(wd => wd.Employee).WithMany(e => e.WorkingDays).HasForeignKey(wd => wd.EmployeeId);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.AppointmentId);
                entity.Property(a => a.UserId).IsRequired();
                entity.Property(a => a.SalonId).IsRequired();
                entity.Property(a => a.ServiceId).IsRequired();
                entity.Property(a => a.EmployeeId).IsRequired();
                entity.Property(a => a.TimeId).IsRequired();
                entity.Property(a => a.Description).IsRequired();

                entity.HasOne(a => a.User).WithMany(u => u.Appointments).HasForeignKey(a => a.UserId);
                entity.HasOne(a => a.Salon).WithMany(s => s.Appointments).HasForeignKey(a => a.SalonId);
                entity.HasOne(a => a.Service).WithMany(sr => sr.Appointments).HasForeignKey(a => a.ServiceId);
                entity.HasOne(a => a.Employee).WithMany(e => e.Appointments).HasForeignKey(a => a.EmployeeId);
                entity.HasOne(a => a.Time).WithMany(t => t.Appointments).HasForeignKey(a => a.TimeId);
            });
        }
    }
}