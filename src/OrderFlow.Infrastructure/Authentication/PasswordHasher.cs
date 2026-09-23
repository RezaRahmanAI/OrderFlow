using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OrderFlow.Application.Abstractions;

namespace OrderFlow.Infrastructure.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new(
        Options.Create(new PasswordHasherOptions { IterationCount = 210_000 }));
    private readonly object _user = new();

    public string Hash(string password) => _hasher.HashPassword(_user, password);

    public bool Verify(string password, string passwordHash)
    {
        return _hasher.VerifyHashedPassword(_user, passwordHash, password)
            != PasswordVerificationResult.Failed;
    }
}
