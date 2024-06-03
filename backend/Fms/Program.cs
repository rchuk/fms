using Fms;

var builder = WebApplication.CreateBuilder(args);
Startup.CreateConfiguration(builder.Configuration);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);

app.Run();