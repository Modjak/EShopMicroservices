using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data.Interceptors;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration confgiuration)
    {
        var connectionString = confgiuration.GetConnectionString("Database");

        //ADD services to the container
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.AddInterceptors(new AuditableEntityInterceptor());
        });

        //services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
