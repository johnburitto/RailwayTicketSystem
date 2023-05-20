namespace Core.Entities
{
    public class Route : Audit
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }

        public TimeSpan TravelTime => ArrivalTime - DepartureTime;
        public List<Train> Trains { get; set; }
    }
}
