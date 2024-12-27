using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class WorkingDay
    {
        public WorkingDay()
        {
        }

        public WorkingDay(int workingDayId, int employeeId, Employee employee, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            WorkingDayId = workingDayId;
            EmployeeId = employeeId;
            Employee = employee;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int WorkingDayId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public Employee Employee { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }
}
