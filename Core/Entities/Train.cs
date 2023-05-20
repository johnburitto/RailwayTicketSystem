namespace Core.Entities
{
    public class Train : Audit
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public int RouteId { get; set; }
        public Route Route { get; set; }
        public List<TrainCar> TrainCars { get; set; }
    }
}
