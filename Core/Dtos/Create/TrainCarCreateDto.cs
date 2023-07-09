using Core.Entities;

namespace Core.Dtos.Create
{
    public class TrainCarCreateDto
    {
        public string Number { get; set; }
        public int TrainId { get; set; }
        public TrainCarType TrainCarType { get; set; }
    }
}
