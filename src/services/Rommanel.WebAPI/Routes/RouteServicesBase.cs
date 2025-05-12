using DevPack.Domain.Mediator;

namespace Rommanel.WebAPI.Routes;

public class RouteServicesBase
{
    public IMediatorHandler? Mediator { get; set; } = default;

}