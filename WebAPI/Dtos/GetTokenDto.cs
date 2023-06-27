namespace WebAPI.Dtos
{
    public class GetTokenDto
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
