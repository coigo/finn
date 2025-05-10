namespace Infra.Http.Middlewares;

public class BasePathEnforcer {

    public RequestDelegate _next;
    public string _pathPath;

    public BasePathEnforcer (RequestDelegate next, string basePath) {
        _next = next;
        _pathPath = basePath;
    } 

    public async Task InvokeAsync (HttpContext context) {

        if (!context.Request.Path.StartsWithSegments(_pathPath)) {
            Console.WriteLine(context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }
        
        await _next(context);
    }

}