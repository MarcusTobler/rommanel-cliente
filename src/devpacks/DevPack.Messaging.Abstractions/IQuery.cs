using MediatR;

namespace DevPack.Messaging.Abstractions;

public interface IQuery : IRequest<Result>;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;