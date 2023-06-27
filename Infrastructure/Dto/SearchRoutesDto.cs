namespace Infrastructure.Dtos
{
    public class SearchRoutesDto
    {
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
