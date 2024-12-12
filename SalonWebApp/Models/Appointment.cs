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

        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
        public int WorkingTimesId { get; set; }
        public WorkingDay WorkingTimes { get; set; }
        public int HoursId { get; set; }
        public Hours Hours { get; set; }
        public string Description { get; set; }
    }
}
