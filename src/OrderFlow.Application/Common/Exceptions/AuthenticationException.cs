namespace OrderFlow.Application.Common.Exceptions;

public sealed class AuthenticationException : Exception
{
    public AuthenticationException() : base("Invalid email or password.")
    {
    }

    public AuthenticationException(string message) : base(message)
    {
    }
}
