using MediatR;

namespace Application.Common;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}