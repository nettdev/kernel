using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Mobnet.SharedKernel;

[ExcludeFromCodeCoverage]
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(UnauthenticatedException)
        {
            await CreateResponse(context, HttpStatusCode.Unauthorized);
        }
        catch(UnauthorizedException)
        {
            await CreateResponse(context, HttpStatusCode.Forbidden);
        }
        catch (ValidationException exception)
        {
            await CreateResponse(context, HttpStatusCode.BadRequest, exception);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task CreateResponse(HttpContext context, HttpStatusCode statusCode, ValidationException? exception = null)
    {
        context.Response.StatusCode = ((int)statusCode);
        await context.Response.WriteAsJsonAsync(exception?.Errors ?? Enumerable.Empty<ValidationFailure>());
    }
}