using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class Service
    {
        public Service()
        {
        }

        public Service(int serviceId, string name, decimal price, TimeSpan duration, int salonId, Salon salon, ICollection<EmployeeService> employeeServices, ICollection<Appointment> appointments)
        {
            ServiceId = serviceId;
            Name = name;
            Price = price;
            Duration = duration;
            SalonId = salonId;
            Salon = salon;
            EmployeeServices = employeeServices;
            Appointments = appointments;
        }

        public int ServiceId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public int? SalonId { get; set; }

        public Salon? Salon { get; set; }

        public ICollection<EmployeeService>? EmployeeServices { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
