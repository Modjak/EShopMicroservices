using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration confgiuration)
    {
        var connectionString = confgiuration.GetConnectionString("Database");


        //ADD services to the container
        //services.AddDbContext<ApplicationDbContext>(
        //    options.UseSqlServer(connectionString));

        //services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
