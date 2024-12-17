using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class Service
    {
        public Service(int serviceId, string name, decimal price, TimeSpan duration, int salonId, Salon salon, ICollection<EmployeeService> employeeServices)
        {
            ServiceId = serviceId;
            Name = name;
            Price = price;
            Duration = duration;
            SalonId = salonId;
            Salon = salon;
            EmployeeServices = employeeServices;
        }

        public int ServiceId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public int SalonId { get; set; }

        [Required]
        public Salon Salon { get; set; }

        public required ICollection<EmployeeService> EmployeeServices { get; set; }
    }
}
