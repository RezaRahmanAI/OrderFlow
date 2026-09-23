namespace OrderFlow.Application.Abstractions;

public sealed record GeneratedRefreshToken(
    string Token,
    string TokenHash,
    DateTime ExpiresAt);

public interface IRefreshTokenGenerator
{
    GeneratedRefreshToken Generate(Guid userId);
}
