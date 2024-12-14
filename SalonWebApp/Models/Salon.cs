namespace SalonWebApp.Models
{
    public class Salon
    {
        public Salon()
        {
        }

        public Salon(int salonId, string name, SalonType type, TimeSpan openingHour, TimeSpan closingHour, HashSet<DayOfWeek> openDays, string address, string phone, ICollection<Employee> employees, ICollection<Service> services, ICollection<Appointment>? appointments)
        {
            SalonId = salonId;
            Name = name;
            Type = type;
            OpeningHour = openingHour;
            ClosingHour = closingHour;
            OpenDays = openDays;
            Address = address;
            Phone = phone;
            Employees = employees;
            Services = services;
            Appointments = appointments;
        }

        public required int SalonId { get; set; }
        public required string Name { get; set; }
        public required SalonType Type { get; set; }
        public required TimeSpan OpeningHour { get; set; }
        public required TimeSpan ClosingHour { get; set; }
        public required HashSet<DayOfWeek> OpenDays { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }

        public required ICollection<Employee> Employees { get; set; }
        public required ICollection<Service> Services { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }

        public bool IsOpen(DateTime dateTime)
        {
            var day = dateTime.DayOfWeek;
            var time = dateTime.TimeOfDay;
            return OpenDays.Contains(day) && time >= OpeningHour && time <= ClosingHour;
        }
    }
}
