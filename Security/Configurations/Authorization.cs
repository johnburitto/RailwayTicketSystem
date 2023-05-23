namespace Security.Configurations
{
    public class Authorization
    {
        public enum Role
        {
            User,
            Admin
        }

        public static readonly Role DEFAULT_ROLE = Role.User;
    }
}
