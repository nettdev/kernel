using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Mobnet.SharedKernel;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAcessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAcessor)
    {
        _httpContextAcessor = httpContextAcessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        var authorizationAttribute = Attribute.GetCustomAttribute(request.GetType(), typeof(AuthorizeAttribute)) as AuthorizeAttribute;
        var user = _httpContextAcessor.HttpContext?.User;
        var resources = user?.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;

        if (authorizationAttribute is not null)
        {
            if (resources is null)
                throw new UnauthenticatedException();

            if (!resources.Split(";").Contains(authorizationAttribute.Resource))
                throw new UnauthorizedException();
        }

        return await next();
    }
}