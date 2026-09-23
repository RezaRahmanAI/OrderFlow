using OrderFlow.Application.Abstractions;

namespace OrderFlow.Application.Authentication.Logout;

public sealed class LogoutService
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IRefreshTokenHasher _hasher;

    public LogoutService(
        IRefreshTokenRepository refreshTokens,
        IRefreshTokenHasher hasher)
    {
        _refreshTokens = refreshTokens;
        _hasher = hasher;
    }

    public async Task ExecuteAsync(LogoutRequest request, CancellationToken cancellationToken = default)
    {
        var tokenHash = _hasher.Hash(request.RefreshToken);
        var token = await _refreshTokens.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (token is not null && token.IsActive)
        {
            token.Revoke();
            await _refreshTokens.SaveChangesAsync(cancellationToken);
        }
    }
}
