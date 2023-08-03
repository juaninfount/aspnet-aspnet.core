using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
Serilog.Log.Logger = new LoggerConfiguration()
.MinimumLevel.Information()
.WriteTo.File("logs/myBeautifulLog-.txt", rollingInterval: RollingInterval.Day)
.CreateLogger(); 
*/

Serilog.Log.Logger = new LoggerConfiguration().
ReadFrom.Configuration(builder.Configuration).
CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
