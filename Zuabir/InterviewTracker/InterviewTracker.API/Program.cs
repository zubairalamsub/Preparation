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
        // Check if database exists
        var canConnect = await db.Database.CanConnectAsync();

        if (!canConnect)
        {
            Console.WriteLine("Database does not exist. Creating...");
            await db.Database.MigrateAsync();
        }
        else
        {
            // Get pending migrations
            var pendingMigrations = await db.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                Console.WriteLine($"Applying {pendingMigrations.Count()} pending migrations...");
                await db.Database.MigrateAsync();
                Console.WriteLine("Migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("Database is up to date. No migrations to apply.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration error: {ex.Message}");
        Console.WriteLine("Attempting to ensure database created...");
        try
        {
            await db.Database.EnsureCreatedAsync();
            Console.WriteLine("Database created successfully.");
        }
        catch (Exception ensureEx)
        {
            Console.WriteLine($"Error creating database: {ensureEx.Message}");
            throw;
        }
    }
}

// Configure pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();
