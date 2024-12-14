namespace SalonWebApp.Models
{
    public class WorkingDay
    {
        public WorkingDay()
        {
        }

        public WorkingDay(int workingDayId, int employeeId, Employee employee, DateTime date, TimeSpan startTime, TimeSpan endTime, ICollection<Appointment> appointments, ICollection<Hours> hours)
        {
            WorkingDayId = workingDayId;
            EmployeeId = employeeId;
            Employee = employee;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Appointments = appointments;
            Hours = hours;
        }

        public required int WorkingDayId { get; set; }
        public required int EmployeeId { get; set; }
        public required Employee Employee { get; set; }
        public required DateTime Date { get; set; }
        public required TimeSpan StartTime { get; set; }
        public required TimeSpan EndTime { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
        public required ICollection<Hours> Hours { get; set; }
    }
}
