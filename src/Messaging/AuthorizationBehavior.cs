using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;

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
        var accountIdHeader = _httpContextAcessor?.HttpContext?.Request?.Headers["x-account"] ?? string.Empty;
        var token = _httpContextAcessor?.HttpContext?.Request?.Headers["Authorization"] ?? string.Empty;
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
}