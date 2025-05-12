using DevPack.Domain.Mediator;

namespace Rommanel.WebAPI.Routes;

public class RouteServicesBase()
{
    protected IMediatorHandler? Mediator { get; set; } = default;

}