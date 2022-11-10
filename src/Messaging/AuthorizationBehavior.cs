using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Mobnet.SharedKernel;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAcessor;
    private readonly ITokenManager _tokenManager;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAcessor, ITokenManager tokenManager)
    {
        _httpContextAcessor = httpContextAcessor;
        _tokenManager = tokenManager;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorization = Attribute.GetCustomAttribute(request.GetType(), typeof(AuthorizeAttribute)) as AuthorizeAttribute;
        var accountIdHeader = GetHeader(_httpContextAcessor?.HttpContext, "x-tentant");
        var token = GetHeader(_httpContextAcessor?.HttpContext, "Authorization");
        var claims = await _tokenManager.GetClaims(token);
        var resources = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;
        var accountId = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.GroupSid))?.Value ?? string.Empty;

        if (authorization is not null)
        {
            if (resources is null)
                throw new UnauthenticatedException();

            if (authorization.CheckTentantAccess && !accountId.Equals(accountIdHeader))
                throw new UnauthorizedException();

            if (!resources.Split(";").Contains(authorization.Resource))
                throw new UnauthorizedException();
        }

        return await next();
    }

    private string GetHeader(HttpContext? context, string key)
    {
        if (context?.Request?.Headers[key] is StringValues value)
            return value.ToString();

        return string.Empty;
    }
}