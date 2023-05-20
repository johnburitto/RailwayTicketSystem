using Core.Entities;

namespace Core.Dtos.Read
{
    public class RouteReadDto
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public TimeSpan TravelTime => ArrivalTime - DepartureTime;
        public List<Train> Trains { get; set; }
    }
}
