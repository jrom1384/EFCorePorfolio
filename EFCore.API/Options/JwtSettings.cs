namespace EFCore.API.Options
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public int ExpirationInMinutes { get; set; }
    }
}
