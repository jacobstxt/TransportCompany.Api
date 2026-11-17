namespace TransportCompany.Api.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Order> Orders { get; set; } = [];
    }
}
