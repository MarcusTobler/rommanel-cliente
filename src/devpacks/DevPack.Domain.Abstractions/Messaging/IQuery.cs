using MediatR;

namespace DevPack.Domain.Abstractions.Messaging;

public interface IQuery : IRequest<Result>
{
    
}

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}

public interface IBaseQuery
{
    
}