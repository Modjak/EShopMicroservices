using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

// Add services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));

});


builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    //opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate; by default it is true
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName); // Set username as a primary key
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
     

/* -----------------   End of adding services-----------------------------  */
var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapCarter();


/* -----------------   End of config of request pipeline-----------------------------  */


app.Run();
