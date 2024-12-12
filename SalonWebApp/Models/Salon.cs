namespace SalonWebApp.Models
{
    public class Salon
    {
        public Salon()
        {
        }

        public Salon(int salonId, string name, SalonType type, string workingHours, string address, string phone, ICollection<Employee> employees, ICollection<Service> services, ICollection<Appointment> appointments)
        {
            SalonId = salonId;
            Name = name;
            Type = type;
            WorkingHours = workingHours;
            Address = address;
            Phone = phone;
            Employees = employees;
            Services = services;
            Appointments = appointments;
        }

        public int SalonId { get; set; }
        public string Name { get; set; }
        public SalonType Type { get; set; }
        public string WorkingHours { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
