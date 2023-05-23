using Core.Entities;

namespace Core.Dtos.Read
{
    public class TicketReadDto
    {
        public DateTime BookDate { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }
        public bool IsAvaliable => (Place.TrainCar.Train.Route.DepartureTime - DateTime.Now).TotalSeconds > 0;
    }
}
