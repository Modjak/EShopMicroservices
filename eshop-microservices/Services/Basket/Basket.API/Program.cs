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

/* -----------------   End of adding services-----------------------------  */
var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapCarter();


/* -----------------   End of config of request pipeline-----------------------------  */


app.Run();
