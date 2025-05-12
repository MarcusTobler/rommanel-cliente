using MediatR;

namespace DevPack.Domain.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
    
}