namespace Core.Entities
{
    public class TrainCar : Audit
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public int TrainId { get; set; }
        public Train Train { get; set; }
        public List<Place> Places { get; set; }
    }
}
