namespace SalonWebApp.Models
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(int employeeId, string firstName, string lastName, HashSet<SpecializationType> specializations, decimal salary, string phone, int salonId, Salon salon, ICollection<WorkingDay> workingTimes, ICollection<Appointment> appointments)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Specializations = specializations;
            Salary = salary;
            Phone = phone;
            SalonId = salonId;
            Salon = salon;
            WorkingDays = workingTimes;
            Appointments = appointments;
        }

        public required int EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required HashSet<SpecializationType> Specializations { get; set; }
        public required decimal Salary { get; set; }
        public required string Phone { get; set; }
        public required int SalonId { get; set; }
        public required Salon Salon { get; set; }

        public required ICollection<WorkingDay> WorkingDays { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
