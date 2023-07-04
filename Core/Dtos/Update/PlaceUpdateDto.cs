using Core.Entities;

namespace Core.Dtos.Update
{
    public class PlaceUpdateDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public float Price { get; set; }
        public PlaceType PlaceType { get; set; }
        public bool IsAvaliable { get; set; }
        public int TrainCarId { get; set; }
    }
}
