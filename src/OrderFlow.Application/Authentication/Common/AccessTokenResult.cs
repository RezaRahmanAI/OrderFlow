namespace OrderFlow.Application.Authentication.Common;

public sealed record AccessTokenResult(string Token, DateTime ExpiresAt);
