namespace Core.Entities
{
    public class Place : Audit
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public float Price { get; set; }
        public PlaceType PlaceType { get; set; }
        public bool IsAvaliable { get; set; }

        public int TrainCarId { get; set; }
        public TrainCar TrainCar { get; set; }
        public List<Ticket> Tickets { get; set; }
    }

    public enum PlaceType
    {
        Compartment,
        Public,
        Luxe,
        S1,
        S2
    }
}
