namespace FsCron.TestApp.Transport;

internal static class TestApi
{
    public static RouteGroupBuilder MapApiEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api");
        return group;
    }
}