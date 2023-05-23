using Core.Entities;

namespace Core.Dtos.Read
{
    public class TrainCarReadDto
    {
        public string Number { get; set; }
        public int TrainId { get; set; }
        public Train Train { get; set; }
        public List<Place> Places { get; set; }
    }
}
