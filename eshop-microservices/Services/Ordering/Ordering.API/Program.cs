using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// ADD services to the contaienr

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();


// Configure the http request pipeline


app.Run();
