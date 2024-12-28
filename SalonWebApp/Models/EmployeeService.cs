using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class EmployeeService
    {
        public EmployeeService()
        {
        }

        public EmployeeService(int employeeServiceId, int employeeId, Employee employee, int serviceId, Service service)
        {
            EmployeeServiceId = employeeServiceId;
            EmployeeId = employeeId;
            Employee = employee;
            ServiceId = serviceId;
            Service = service;
        }

        public int EmployeeServiceId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public Service? Service { get; set; }
    }
}
