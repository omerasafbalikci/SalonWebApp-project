namespace SalonWebApp.Models
{
    public class Appointment
    {
        public Appointment()
        {
        }

        public Appointment(int appointmentId, int userId, User user, int serviceId, Service service, int employeeId, Employee employee, int salonId, Salon salon, int workingTimesId, WorkingDay workingTimes, int hoursId, Hours hours, string description)
        {
            AppointmentId = appointmentId;
            UserId = userId;
            User = user;
            ServiceId = serviceId;
            Service = service;
            EmployeeId = employeeId;
            Employee = employee;
            SalonId = salonId;
            Salon = salon;
            WorkingTimesId = workingTimesId;
            WorkingTimes = workingTimes;
            HoursId = hoursId;
            Hours = hours;
            Description = description;
        }

        public required int AppointmentId { get; set; }
        public required int UserId { get; set; }
        public required User User { get; set; }
        public required int ServiceId { get; set; }
        public required Service Service { get; set; }
        public required int EmployeeId { get; set; }
        public required Employee Employee { get; set; }
        public required int SalonId { get; set; }
        public required Salon Salon { get; set; }
        public required int WorkingTimesId { get; set; }
        public required WorkingDay WorkingTimes { get; set; }
        public required int HoursId { get; set; }
        public required Hours Hours { get; set; }
        public required string Description { get; set; }
    }
}
