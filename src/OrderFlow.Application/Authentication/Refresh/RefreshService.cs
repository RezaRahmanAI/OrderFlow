using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Authentication.Common;
using OrderFlow.Application.Common.Exceptions;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Authentication.Refresh;

public sealed class RefreshService
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IRefreshTokenHasher _hasher;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUserRepository _users;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshService(
        IRefreshTokenRepository refreshTokens,
        IRefreshTokenHasher hasher,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUserRepository users,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _refreshTokens = refreshTokens;
        _hasher = hasher;
        _refreshTokenGenerator = refreshTokenGenerator;
        _users = users;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> ExecuteAsync(RefreshRequest request, CancellationToken cancellationToken = default)
    {
        var tokenHash = _hasher.Hash(request.RefreshToken);
        var existingToken = await _refreshTokens.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (existingToken is null || !existingToken.IsActive)
        {
            throw new AuthenticationException("Invalid or expired refresh token.");
        }

        var user = await _users.GetByIdAsync(existingToken.UserId, cancellationToken);
        if (user is null || !user.IsActive)
        {
            throw new AuthenticationException("Invalid or expired refresh token.");
        }

        var newAccessToken = _jwtTokenGenerator.Generate(user);
        var newRefreshToken = _refreshTokenGenerator.Generate(user.Id);

        var replacementToken = new RefreshToken(
            user.Id,
            newRefreshToken.TokenHash,
            newRefreshToken.ExpiresAt);

        await _refreshTokens.AddAsync(replacementToken, cancellationToken);
        existingToken.Revoke(replacementToken.Id);

        await _refreshTokens.SaveChangesAsync(cancellationToken);

        return new AuthResponse(
            user.Id,
            user.Email,
            user.Role.ToString(),
            newAccessToken.Token,
            newAccessToken.ExpiresAt,
            newRefreshToken.Token,
            newRefreshToken.ExpiresAt);
    }
}
