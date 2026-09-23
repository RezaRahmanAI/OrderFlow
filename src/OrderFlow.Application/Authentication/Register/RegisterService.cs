using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Authentication.Common;
using OrderFlow.Application.Common.Exceptions;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Authentication.Register;

public sealed class RegisterService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokens;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RegisterService(
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

    public async Task<AuthResponse> ExecuteAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _users.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ConflictException("A user with this email already exists.");

        var user = new User(request.Email, _passwordHasher.Hash(request.Password));
        await _users.AddAsync(user, cancellationToken);

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
