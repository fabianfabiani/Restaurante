namespace Restaurante.Entities
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        public string SecretKey { get; set; }
    }
}
