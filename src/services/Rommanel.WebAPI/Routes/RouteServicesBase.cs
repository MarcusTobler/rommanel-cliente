using DevPack.Domain.Mediator;

namespace Rommanel.WebAPI.Routes;

public class RouteServicesBase(IHttpContextAccessor httpContextAccessor)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    protected IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;
    
    protected IMediatorHandler? Mediator { get; set; } = default;

}