namespace SalonWebApp.Models
{
    public class User
    {
        public User()
        {
        }

        public User(int userId, string firstName, string lastName, string password, string email, string phoneNumber, Gender gender, Roles role, ICollection<Appointment>? appointments)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Role = role;
            Appointments = appointments;
        }

        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required Gender Gender { get; set; }
        public required Roles Role { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
