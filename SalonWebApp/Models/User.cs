namespace SalonWebApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
