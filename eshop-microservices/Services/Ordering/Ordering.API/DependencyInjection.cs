namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        //services.AddCartes();
        return services;
    }

    public static WebApplication useApiServices(this WebApplication app)
    {
        //app.MapCarter();

        return app;
    }
}
