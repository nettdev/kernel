namespace Nett.Kernel;

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
        catch (AuthenticationException)
        {
            await CreateResponse(context, 401);
        }
        catch (AuthorizationException exception)
        {
            await CreateResponse(context, 403, exception);
        }
        catch (ValidationException exception)
        {
            await CreateResponse(context, 400, exception);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task CreateResponse(HttpContext context, int statusCode, ValidationException? ex = null)
    {
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(ex?.Errors ?? Enumerable.Empty<ValidationFailure>());
    }
}