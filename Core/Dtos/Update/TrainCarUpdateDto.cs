using Core.Entities;

namespace Core.Dtos.Update
{
    public class TrainCarUpdateDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int TrainId { get; set; }
        public TrainCarType TrainCarType { get; set; }
    }
}
