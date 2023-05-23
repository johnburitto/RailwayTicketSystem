using Core.Entities;

namespace Core.Dtos.Read
{
    public class PlaceReadDto
    {
        public float Price { get; set; }
        public PlaceType PlaceType { get; set; }
        public bool IsAvaliable { get; set; }
        public int TrainCarId { get; set; }
        public TrainCar TrainCar { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
