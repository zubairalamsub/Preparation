using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Models;
using System.Text.Json;

namespace InterviewTracker.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DSAProblem> DSAProblems => Set<DSAProblem>();
    public DbSet<SystemDesignTopic> SystemDesignTopics => Set<SystemDesignTopic>();
    public DbSet<MockInterview> MockInterviews => Set<MockInterview>();
    public DbSet<WeakArea> WeakAreas => Set<WeakArea>();
    public DbSet<StudySession> StudySessions => Set<StudySession>();

    // New DbSets for additional learning sections
    public DbSet<AzureTopic> AzureTopics => Set<AzureTopic>();
    public DbSet<OOPTopic> OOPTopics => Set<OOPTopic>();
    public DbSet<CSharpTopic> CSharpTopics => Set<CSharpTopic>();
    public DbSet<AspNetCoreTopic> AspNetCoreTopics => Set<AspNetCoreTopic>();
    public DbSet<SqlServerTopic> SqlServerTopics => Set<SqlServerTopic>();
    public DbSet<DesignPatternTopic> DesignPatternTopics => Set<DesignPatternTopic>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure List<string> to be stored as JSON
        modelBuilder.Entity<DSAProblem>()
            .Property(p => p.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        modelBuilder.Entity<SystemDesignTopic>()
            .Property(p => p.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        modelBuilder.Entity<AzureTopic>()
            .Property(p => p.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        modelBuilder.Entity<OOPTopic>()
            .Property(p => p.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        modelBuilder.Entity<CSharpTopic>()
            .Property(p => p.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        modelBuilder.Entity<AspNetCoreTopic>()
            .Property(p => p.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        modelBuilder.Entity<SqlServerTopic>()
            .Property(p => p.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        modelBuilder.Entity<DesignPatternTopic>()
            .Property(p => p.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );
    }
}
