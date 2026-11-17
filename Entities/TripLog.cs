namespace TransportCompany.Api.Entities
{
    public class TripLog
    {
        public int Id { get; set; }
        public DateTime ActualTime { get; set; }
        public string? DriverComment { get; set; }
        public int TripDetailsId { get; set; }
        public TripDetails TripDetails { get; set; } = null!;
    }
}
