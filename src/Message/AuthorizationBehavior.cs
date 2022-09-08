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

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var authorizationAttribute = Attribute.GetCustomAttribute(request.GetType(), typeof(AuthorizeAttribute)) as AuthorizeAttribute;
        var permissions = _httpContextAcessor.HttpContext?.User.FindFirst(c => c.Type.Equals("permissions"))?.Value;

        if (authorizationAttribute is not null)
        {
            if (permissions is null)
                throw new UnauthenticatedException();

            if (!permissions.Split(";").Contains(authorizationAttribute.Permission))
                throw new UnauthorizedAccessException();
        }

        return await next();
    }
}