namespace SalonWebApp.Models
{
    public class Hours
    {
        public Hours()
        {
        }

        public Hours(int hoursId, int workingDayId, WorkingDay workingDay, TimeSpan timeZone, bool selectable, ICollection<Appointment> appointments)
        {
            HoursId = hoursId;
            WorkingDayId = workingDayId;
            WorkingDay = workingDay;
            TimeZone = timeZone;
            Selectable = selectable;
            Appointments = appointments;
        }

        public int HoursId { get; set; }
        public int WorkingDayId { get; set; }
        public WorkingDay WorkingDay { get; set; }
        public TimeSpan TimeZone { get; set; }
        public bool Selectable { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
