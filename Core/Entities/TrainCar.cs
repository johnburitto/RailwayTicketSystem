namespace Core.Entities
{
    public class TrainCar : Audit
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public TrainCarType TrainCarType { get; set; }

        public int TrainId { get; set; }
        public Train Train { get; set; }
        public List<Place> Places { get; set; }
    }

    public enum TrainCarType
    {
        Compartment,
        Public,
    }
}
