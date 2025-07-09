using JobUpdatesAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<JobUpdatesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapControllers();

// Apply DB migration automatically
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<JobUpdatesDbContext>();
db.Database.Migrate();

app.Run();
