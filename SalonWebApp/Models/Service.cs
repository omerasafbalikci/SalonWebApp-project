namespace SalonWebApp.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public required string ServiceType { get; set; }
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }
        public int SalonId { get; set; }
        public required Salon Salon { get; set; }
    }
}
