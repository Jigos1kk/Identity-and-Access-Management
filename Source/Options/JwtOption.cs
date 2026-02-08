using System;

namespace Source.Options;

public class JwtOption
{
    public const string Jwt = "Jwt";
    public required string Secret { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
