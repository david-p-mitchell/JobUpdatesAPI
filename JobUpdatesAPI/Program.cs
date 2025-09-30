using JobUpdatesAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<JobUpdatesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // default route is /swagger
}

// Map controller endpoints
app.MapControllers();

// Apply DB migration automatically
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<JobUpdatesDbContext>();
db.Database.Migrate();

app.Run();
