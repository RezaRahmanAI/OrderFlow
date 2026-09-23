using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Authentication.Common;
using OrderFlow.Application.Common.Exceptions;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Authentication.Login;

public sealed class LoginService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokens;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginService(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator tokens,
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _tokens = tokens;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthResponse> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || !user.IsActive || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new AuthenticationException();

        var token = _tokens.Generate(user);
        var generatedRefreshToken = _refreshTokenGenerator.Generate(user.Id);

        var refreshToken = new RefreshToken(
            user.Id,
            generatedRefreshToken.TokenHash,
            generatedRefreshToken.ExpiresAt);

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return new AuthResponse(
            user.Id,
            user.Email,
            user.Role.ToString(),
            token.Token,
            token.ExpiresAt,
            generatedRefreshToken.Token,
            generatedRefreshToken.ExpiresAt);
    }
}
