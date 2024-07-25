var builder = WebApplication.CreateBuilder(args);


//Add services to the container.

var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));

});

builder.Services.AddValidatorsFromAssembly(assembly); // scans the assembly and if there are validators of that type it will register to the container of the project

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();


//Configure the Http request pipeline
app.MapCarter();  // scans the ALL classes that implements ICarterModule and maps the required HTTP methods

app.UseExceptionHandler(options => { });


app.Run();
