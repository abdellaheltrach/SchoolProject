namespace School.Domain.Options
{
    public class JwtSettings
    {
        public string SecretKey { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public bool ValidateIssuer { get; init; }
        public bool ValidateAudience { get; init; }
        public bool ValidateLifeTime { get; init; }
        public bool ValidateIssuerSigningKey { get; init; }
        public int AccessTokenExpirationTimeInMinutes { get; init; }
        public int RefreshTokenExpirationTimeInDays { get; init; }
    }
}
