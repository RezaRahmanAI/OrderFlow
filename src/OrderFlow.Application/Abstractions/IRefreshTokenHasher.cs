namespace OrderFlow.Application.Abstractions;

public interface IRefreshTokenHasher
{
    string Hash(string token);
}
