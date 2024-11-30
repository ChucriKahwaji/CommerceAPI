var builder = WebApplication.CreateBuilder(args);

// Add services to the container using Startup.cs
var startup = new API.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline using Startup.cs
startup.Configure(app, app.Environment);

app.Run();