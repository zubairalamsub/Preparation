namespace InterviewTracker.API.Models;

public class DSAProblem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Array, String, Tree, Graph, DP, etc.
    public string Difficulty { get; set; } = string.Empty; // Easy, Medium, Hard
    public string Platform { get; set; } = string.Empty; // LeetCode, HackerRank, etc.
    public string? ProblemUrl { get; set; }
    public string Status { get; set; } = "NotStarted"; // NotStarted, InProgress, Solved, NeedsReview
    public int TimeTakenMinutes { get; set; }
    public bool SolvedOptimally { get; set; }
    public string? Notes { get; set; }
    public string? SolutionApproach { get; set; }
    public string? TimeComplexity { get; set; }
    public string? SpaceComplexity { get; set; }
    public int AttemptCount { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastAttemptedAt { get; set; }
    public DateTime? NextReviewDate { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
    public int? LeetCodeNumber { get; set; }
}

public class SystemDesignTopic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Scalability, Database, Caching, etc.
    public string Difficulty { get; set; } = string.Empty;
    public string Status { get; set; } = "NotStarted"; // NotStarted, Learning, Understood, Mastered
    public int ConfidenceLevel { get; set; } // 1-5
    public string? Notes { get; set; }
    public string? KeyConcepts { get; set; }
    public string? Lesson { get; set; } // Detailed lesson content
    public string? DiagramUrl { get; set; } // Architecture diagram URL
    public string? Resources { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
}

public class MockInterview
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // DSA, SystemDesign, Behavioral
    public string Company { get; set; } = string.Empty;
    public DateTime InterviewDate { get; set; }
    public int DurationMinutes { get; set; }
    public int OverallScore { get; set; } // 1-10
    public int CommunicationScore { get; set; } // 1-10
    public int ProblemSolvingScore { get; set; } // 1-10
    public int TechnicalScore { get; set; } // 1-10
    public string? Feedback { get; set; }
    public string? Strengths { get; set; }
    public string? AreasToImprove { get; set; }
    public string? QuestionsAsked { get; set; }
    public bool Passed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class WeakArea
{
    public int Id { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // DSA, SystemDesign, Behavioral
    public string Severity { get; set; } = "Medium"; // Low, Medium, High
    public string? Description { get; set; }
    public string? ImprovementPlan { get; set; }
    public bool IsResolved { get; set; }
    public DateTime IdentifiedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
}

public class StudySession
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // DSA, SystemDesign, Behavioral
    public string Topic { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int ProductivityScore { get; set; } // 1-5
    public string? Notes { get; set; }
    public DateTime SessionDate { get; set; } = DateTime.UtcNow;
}

// New Models for Additional Learning Sections

public class AzureTopic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Status { get; set; } = "NotStarted";
    public int ConfidenceLevel { get; set; }
    public string? Notes { get; set; }
    public string? KeyConcepts { get; set; }
    public string? Lesson { get; set; } // Detailed lesson content
    public string? CodeExample { get; set; }
    public string? Resources { get; set; }
    public string? AzureService { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
}

public class OOPTopic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Status { get; set; } = "NotStarted";
    public int ConfidenceLevel { get; set; }
    public string? Notes { get; set; }
    public string? KeyConcepts { get; set; }
    public string? Lesson { get; set; } // Detailed lesson content
    public string? CodeExample { get; set; }
    public string? Resources { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
}

public class CSharpTopic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Status { get; set; } = "NotStarted";
    public int ConfidenceLevel { get; set; }
    public string? Notes { get; set; }
    public string? KeyConcepts { get; set; }
    public string? Lesson { get; set; } // Detailed lesson content
    public string? CodeExample { get; set; }
    public string? Resources { get; set; }
    public string? DotNetVersion { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
}

public class AspNetCoreTopic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Status { get; set; } = "NotStarted";
    public int ConfidenceLevel { get; set; }
    public string? Notes { get; set; }
    public string? KeyConcepts { get; set; }
    public string? Lesson { get; set; } // Detailed lesson content
    public string? CodeExample { get; set; }
    public string? Resources { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
}

public class SqlServerTopic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Status { get; set; } = "NotStarted";
    public int ConfidenceLevel { get; set; }
    public string? Notes { get; set; }
    public string? KeyConcepts { get; set; }
    public string? Lesson { get; set; } // Detailed lesson content
    public string? SqlExample { get; set; }
    public string? Resources { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
}

public class DesignPatternTopic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Status { get; set; } = "NotStarted";
    public int ConfidenceLevel { get; set; }
    public string? Notes { get; set; }
    public string? KeyConcepts { get; set; }
    public string? Lesson { get; set; } // Detailed lesson content
    public string? CodeExample { get; set; }
    public string? UseCases { get; set; }
    public string? Resources { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
}

public class EntityFrameworkTopic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Status { get; set; } = "NotStarted";
    public int ConfidenceLevel { get; set; }
    public string? Notes { get; set; }
    public string? KeyConcepts { get; set; }
    public string? Lesson { get; set; } // Detailed lesson content
    public string? CodeExample { get; set; }
    public string? ProblemScenario { get; set; }
    public string? Resources { get; set; }
    public string? EFVersion { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsFavorite { get; set; }
}
