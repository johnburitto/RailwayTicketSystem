namespace Core.Dtos.Update
{
    public class RouteUpdateDto
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
    }
}
