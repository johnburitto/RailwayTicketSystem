using Core.Entities;

namespace Core.Dtos.Read
{
    public class TrainReadDto
    {
        public string Number { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
        public List<TrainCar> TrainCars { get; set; }
    }
}
