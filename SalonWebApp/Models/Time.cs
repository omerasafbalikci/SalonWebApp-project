using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class Time
    {
        public Time(int timeId, DateTime date, TimeSpan startTime, TimeSpan endTime, bool selectable, int appointmentId, Appointment appointments)
        {
            TimeId = timeId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Selectable = selectable;
            AppointmentId = appointmentId;
            Appointments = appointments;
        }

        public required int TimeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public bool Selectable { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public Appointment Appointments { get; set; }
    }
}
