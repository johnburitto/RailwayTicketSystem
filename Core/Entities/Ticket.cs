namespace Core.Entities
{
    public class Ticket : Audit
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime BookDate { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}
