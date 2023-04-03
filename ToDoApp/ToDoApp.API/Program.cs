
using ToDoApp.API.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureAllServices(builder.Configuration);
builder.ConfigureSerilog();

var app = builder.Build();
app.ConfigureAllMiddlewares();
app.Run();