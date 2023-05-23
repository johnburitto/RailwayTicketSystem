using Core.Entities;

namespace Core.Dtos.Create
{
    public class PlaceCreateDto
    {
        public float Price { get; set; }
        public PlaceType PlaceType { get; set; }
        public bool IsAvaliable { get; set; }
        public int TrainCarId { get; set; }
    }
}
