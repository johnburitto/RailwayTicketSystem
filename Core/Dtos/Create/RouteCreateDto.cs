using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Create
{
    public class RouteCreateDto
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
    }
}
