using Common.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.Security;

public abstract class AbstractTokenStore
{
    private TimeSpan DefaultAuthorizationExpiration;
    private TimeSpan DefaultRefreshExpiration;
    private TimeSpan ClockSkew;

    public AbstractTokenStore(TimeSpan defaultAuthoriationExpiration, TimeSpan defaultRefreshExpiration, TimeSpan clockSkew)
    {
        DefaultAuthorizationExpiration = defaultAuthoriationExpiration;
        DefaultRefreshExpiration = defaultRefreshExpiration;
        ClockSkew = clockSkew;
    }

    public string GenerateAuthorizationToken(string username, string[] roles) => GenerateAuthorizationToken(username, roles, DefaultAuthorizationExpiration);
    public string GenerateAuthorizationToken(string username, string[] roles, TimeSpan expiration)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username), // Subject (typically the user's identifier)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token identifier
            new Claim(JwtRegisteredClaimNames.Iss, Config.AuthIssuer), // Issuer
            new Claim(JwtRegisteredClaimNames.Aud, Config.AuthAudience), // Audience
        };

        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = DateTime.UtcNow.Add(expiration), // Token expiration time

            SigningCredentials = SecurityHandler.AuthorizationSigningCredentials
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        LogWriter.LogInfo("Generated new authorization token");

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(string username) => GenerateRefreshToken(username, DefaultRefreshExpiration);
    public string GenerateRefreshToken(string username, TimeSpan expiration)
    {
        string token = Guid.NewGuid().ToString();
        _ = StoreRefreshToken(token, username, DateTime.UtcNow.Add(expiration));

        LogWriter.LogInfo("Generated new refresh token");

        return token;
    }

    public (string authorizationToken, string refreshToken) GenerateTokenSet(string username, string[] roles) => (GenerateAuthorizationToken(username, roles), GenerateRefreshToken(username));

    public bool RemoveAndVerifyRefreshToken(string token, [NotNullWhen(true)] out string? username, [NotNullWhen(true)] out string? newRefreshToken)
    {
        var removed = RemoveRefreshToken(token);

        if (removed.HasValue)
        {
            if (removed.Value.expiration <= DateTime.UtcNow.Add(ClockSkew))
            {
                username = removed.Value.username;
                newRefreshToken = GenerateRefreshToken(username);
                return true;
            }
        }
        
        username = null;
        newRefreshToken = null;
        return false;
    }

    public abstract bool StoreRefreshToken(string token, string username, DateTime expiration);
    public abstract (string token, string username, DateTime expiration)? RemoveRefreshToken(string token);
}
