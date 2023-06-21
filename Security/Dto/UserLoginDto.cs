namespace Security.Dto
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
