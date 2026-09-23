using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using OrderFlow.Application.Abstractions;

namespace OrderFlow.Infrastructure.Authentication;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IRefreshTokenHasher _hasher;

    public RefreshTokenGenerator(
        IOptions<JwtSettings> jwtSettings,
        IRefreshTokenHasher hasher)
    {
        _jwtSettings = jwtSettings.Value;
        _hasher = hasher;
    }

    public GeneratedRefreshToken Generate(Guid userId)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var rawToken = Convert.ToBase64String(randomBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
        var tokenHash = _hasher.Hash(rawToken);
        var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        return new GeneratedRefreshToken(rawToken, tokenHash, expiresAt);
    }
}
