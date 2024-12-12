namespace SalonWebApp.Models
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(int employeeId, string name, string surname, HashSet<SpecializationType> specializations, decimal salary, string phone, int salonId, Salon salon, ICollection<WorkingDay> workingTimes, ICollection<Appointment> appointments)
        {
            EmployeeId = employeeId;
            Name = name;
            Surname = surname;
            Specializations = specializations;
            Salary = salary;
            Phone = phone;
            SalonId = salonId;
            Salon = salon;
            WorkingDays = workingTimes;
            Appointments = appointments;
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public HashSet<SpecializationType> Specializations { get; set; }
        public decimal Salary { get; set; }
        public string Phone { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        public ICollection<WorkingDay> WorkingDays { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
