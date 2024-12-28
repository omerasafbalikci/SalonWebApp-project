using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class Appointment
    {
        public Appointment()
        {
        }

        public Appointment(int appointmentId, int userId, User user, int salonId, Salon salon, int serviceId, Service service, int employeeId, Employee employee, int timeId, Time time, string? description)
        {
            AppointmentId = appointmentId;
            UserId = userId;
            User = user;
            SalonId = salonId;
            Salon = salon;
            ServiceId = serviceId;
            Service = service;
            EmployeeId = employeeId;
            Employee = employee;
            TimeId = timeId;
            Time = time;
            Description = description;
        }

        public int AppointmentId { get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public int SalonId { get; set; }

        public Salon? Salon { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public Service? Service { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        [Required]
        public int TimeId { get; set; }

        public Time? Time { get; set; }

        public string? Description { get; set; }
    }
}
