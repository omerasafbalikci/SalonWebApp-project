using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class Time
    {
        public Time()
        {
        }

        public Time(int timeId, DateTime date, TimeSpan startTime, TimeSpan endTime, bool selectable, ICollection<Appointment> appointments)
        {
            TimeId = timeId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Selectable = selectable;
            Appointments = appointments;
        }

        public int TimeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public bool Selectable { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
