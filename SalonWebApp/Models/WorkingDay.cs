namespace SalonWebApp.Models
{
    public class WorkingDay
    {
        public WorkingDay()
        {
        }

        public WorkingDay(int workingDayId, int employeeId, Employee employee, DateTime date, TimeSpan startTime, DateTime endTime, ICollection<Appointment> appointments, ICollection<Hours> hours)
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

        public int WorkingDayId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Hours> Hours { get; set; }
    }
}
