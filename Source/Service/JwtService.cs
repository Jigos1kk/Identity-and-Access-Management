using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Source.Options;
using Source.ViewModels;

namespace Source.Service;

public class JwtService(IOptions<JwtOption> options)
{
    private readonly JwtOption _options = options.Value;

    public async Task<TokensResponce> GenerateTokensAsync(string userId, string email, bool rememberMe)
    {
        var accessToken = await GenerateAccessTokenAsync(userId, email);
        var refreshToken = await GenerateRefreshTokenAsync(userId, rememberMe);
        return new TokensResponce
        {
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.Token,
            AccessTokenExpires = accessToken.ExpiresAt,
            RefreshTokenExpires = refreshToken.ExpiresAt
        };
    }

    public async Task<TokenResult> GenerateAccessTokenAsync(string userId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.Secret);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

        var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new TokenResult{
            Token = tokenHandler.WriteToken(token),
            ExpiresAt = expires
        };
    }

    public async Task<TokenResult> GenerateRefreshTokenAsync(string userId, bool rememberMe)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.Secret);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

        var expires = DateTime.UtcNow.AddDays(rememberMe ? _options.RefreshTokenExpirationDays : 1);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new TokenResult{
            Token = tokenHandler.WriteToken(token),
            ExpiresAt = expires
        };
    }
}