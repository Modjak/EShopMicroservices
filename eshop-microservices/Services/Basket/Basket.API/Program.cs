using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using Grpc.Net.Client;
using HealthChecks.UI.Client;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

// Add services to the container


//Application Services
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));

});


//Data Services
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    //opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate; by default it is true
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName); // Set username as a primary key
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>(); //a class requires an IBasketRepository in its constructor, the DI container will provide an instance of BasketRepository.
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
});


//Grpc Services

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});


//ASYNC Communication Services

builder.Services.AddMessageBroker(builder.Configuration); //no need assembly since we are in publisher side not consumer

//Cross-cutting concerns
#region How DI Container Resolves Dependencies
/* 
 

Request for IBasketRepository: When GetBasketQueryHandler is created, the DI container sees that it needs an IBasketRepository.
Decorated Implementation: The container knows from Decorate<IBasketRepository, CachedBasketRepository>() that any IBasketRepository request should be fulfilled by CachedBasketRepository.
Constructor Injection: The CachedBasketRepository itself needs an IBasketRepository and an IDistributedCache.
The DI container provides BasketRepository for IBasketRepository.
The DI container provides the configured Redis cache for IDistributedCache.

 */
#endregion

#region Manual Registration and Decoration

// Step 1: Register the Concrete Implementation

/*
 * 
builder.Services.AddScoped<BasketRepository>();
builder.Services.AddScoped<IBasketRepository>(provider =>
{
    return provider.GetRequiredService<BasketRepository>();
});

*/
// Step 2: Register the Decorator

/*
builder.Services.AddScoped<CachedBasketRepository>();
builder.Services.AddScoped<IBasketRepository>(provider =>
{
    // Get the original IBasketRepository service
    var repository = provider.GetRequiredService<BasketRepository>();

    // Get the cache service
    var cache = provider.GetRequiredService<IDistributedCache>();

    // Return a new instance of CachedBasketRepository wrapping the original repository
    return new CachedBasketRepository(repository, cache);
});

*/
#endregion

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

/* -----------------   End of adding services-----------------------------  */

var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapCarter();
app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

/* -----------------   End of config of request pipeline-----------------------------  */


app.Run();
