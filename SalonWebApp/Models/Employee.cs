using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(int employeeId, string firstName, string lastName, decimal salary, string phone, int salonId, Salon salon, ICollection<EmployeeService> employeeServices, ICollection<WorkingDay> workingDays, ICollection<Appointment> appointments)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Salary = salary;
            Phone = phone;
            SalonId = salonId;
            Salon = salon;
            EmployeeServices = employeeServices;
            WorkingDays = workingDays;
            Appointments = appointments;
        }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public int SalonId { get; set; }

        public Salon? Salon { get; set; }

        public ICollection<EmployeeService>? EmployeeServices { get; set; }
        public ICollection<WorkingDay>? WorkingDays { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
