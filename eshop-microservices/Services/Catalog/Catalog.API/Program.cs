var builder = WebApplication.CreateBuilder(args);


//Add services to the container 
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();


var app = builder.Build();


//Configure the Http request pipeline
app.MapCarter(); 
// scans the ALL classes that implements ICarterModule and maps the required HTTP methods


app.Run();
