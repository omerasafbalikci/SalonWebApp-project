using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class EmployeeService
    {
        public EmployeeService(int id, int employeeId, Employee employee, int serviceId, Service service)
        {
            Id = id;
            EmployeeId = employeeId;
            Employee = employee;
            ServiceId = serviceId;
            Service = service;
        }

        public required int Id {  get; set; }

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
