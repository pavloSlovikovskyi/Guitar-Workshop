using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthTokenResult>>;
