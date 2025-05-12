using DevPack.Domain.Abstractions;
using MediatR;

namespace DevPack.Domain.Messaging;

public abstract class Query : IRequest<Result>
{
    
}

public abstract class Query<TResponse> : Query, IRequest<Result<TResponse>>
{
    
}