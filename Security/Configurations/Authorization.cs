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
        public static readonly string COOKIE = "idsrv.cookie";
        public static readonly string COOKIE_C1 = "idsrv.cookieC1";
        public static readonly string COOKIE_C2 = "idsrv.cookieC2";
        public static readonly string COOKIE_ANTIFORGERY = "idsrv.antiforgery";
    }
}
