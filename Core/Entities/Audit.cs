namespace Core.Entities
{
    public class Audit
    {
        public string? ActionUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
    }
}
