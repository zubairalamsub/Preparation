using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL - Support both local and production (Neon/Render)
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=interviewtracker;Username=postgres;Password=postgres";

// Convert Neon/Heroku style DATABASE_URL to Npgsql format if needed
if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
{
    // Remove query string parameters before parsing
    var urlWithoutQuery = connectionString.Split('?')[0];
    var uri = new Uri(urlWithoutQuery);
    var userInfo = uri.UserInfo.Split(':');
    var database = uri.AbsolutePath.TrimStart('/');
    connectionString = $"Host={uri.Host};Port={(uri.Port > 0 ? uri.Port : 5432)};Database={database};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure CORS for Angular - Support both local and production
var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',')
    ?? new[] {
        "http://localhost:4200",
        "https://preparation-pi.vercel.app"
    };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ensure database is created and apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // Apply any pending migrations automatically on startup
        db.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
        // Fallback to EnsureCreated for initial setup
        Console.WriteLine("Falling back to EnsureCreated...");
        db.Database.EnsureCreated();
    }
}

// Configure pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();
