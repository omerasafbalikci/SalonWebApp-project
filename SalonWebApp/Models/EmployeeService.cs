using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class EmployeeService
    {
        public EmployeeService(int employeeServiceId, int employeeId, Employee employee, int serviceId, Service service)
        {
            EmployeeServiceId = employeeServiceId;
            EmployeeId = employeeId;
            Employee = employee;
            ServiceId = serviceId;
            Service = service;
        }

        public required int EmployeeServiceId {  get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public Employee Employee { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public Service Service { get; set; }
    }
}
