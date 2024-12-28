using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class User
    {
        public User()
        {
        }

        public User(int userId, string firstName, string lastName, string password, string email, string phoneNumber, Gender gender, Roles role, ICollection<Appointment> appointments)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Role = role;
            Appointments = new List<Appointment>();
        }

        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public Roles? Role { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
