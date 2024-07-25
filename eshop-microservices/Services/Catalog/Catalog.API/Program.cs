var builder = WebApplication.CreateBuilder(args);


//Add services to the container.

var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    // The order you register this handlers will be run in the same sequence while processing by MediatR
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    config.AddOpenBehavior(typeof(ValidationBehavior<,>)); 
});

builder.Services.AddValidatorsFromAssembly(assembly); // scans the assembly and if there are validators of that type it will register to the container of the project

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

// If docker is not up , we will encounter with error. For now 
// I wont handle this bur TODO
if(builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();


var app = builder.Build();


//Configure the Http request pipeline
app.MapCarter();  // scans the ALL classes that implements ICarterModule and maps the required HTTP methods

app.UseExceptionHandler(options => { });

app.Run();
