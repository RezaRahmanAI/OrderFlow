using OrderFlow.Application.Authentication.Common;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Abstractions;

public interface IJwtTokenGenerator
{
    AccessTokenResult Generate(User user);
}
