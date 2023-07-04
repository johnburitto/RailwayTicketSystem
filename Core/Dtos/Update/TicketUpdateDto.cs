namespace Core.Dtos.Update
{
    public class TicketUpdateDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime BookDate { get; set; }
        public int PlaceId { get; set; }
    }
}
