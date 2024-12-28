using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class Salon
    {
        public Salon()
        {
        }

        public Salon(int salonId, string name, SalonType type, string openDays, TimeSpan openingHour, TimeSpan closingHour, string address, string phone, ICollection<Employee> employees, ICollection<Service> services, ICollection<Appointment> appointments)
        {
            SalonId = salonId;
            Name = name;
            Type = type;
            OpenDays = openDays;
            OpeningHour = openingHour;
            ClosingHour = closingHour;
            Address = address;
            Phone = phone;
            Employees = employees;
            Services = services;
            Appointments = appointments;
        }

        public int SalonId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public SalonType Type { get; set; }

        [Required]
        public string OpenDays { get; set; }

        [Required]
        [Range(typeof(TimeSpan), "00:00:00", "23:59:59")]
        public TimeSpan OpeningHour { get; set; }

        [Required]
        [Range(typeof(TimeSpan), "00:00:00", "23:59:59")]
        public TimeSpan ClosingHour { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

        public bool IsOpen(DateTime dateTime)
        {
            var day = dateTime.DayOfWeek;
            var time = dateTime.TimeOfDay;
            var openDays = OpenDays.Split(',').Select(d => Enum.Parse<DayOfWeek>(d)).ToHashSet();
            return openDays.Contains(day) && time >= OpeningHour && time <= ClosingHour;
        }
    }
}
