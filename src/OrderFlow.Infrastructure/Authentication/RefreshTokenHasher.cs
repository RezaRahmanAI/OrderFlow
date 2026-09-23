using System.Security.Cryptography;
using System.Text;
using OrderFlow.Application.Abstractions;

namespace OrderFlow.Infrastructure.Authentication;

public sealed class RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
