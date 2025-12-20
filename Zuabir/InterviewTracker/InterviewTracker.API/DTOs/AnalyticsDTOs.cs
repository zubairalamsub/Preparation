namespace InterviewTracker.API.DTOs;

public class DashboardStats
{
    public int TotalDSAProblems { get; set; }
    public int SolvedDSAProblems { get; set; }
    public int TotalSystemDesignTopics { get; set; }
    public int MasteredTopics { get; set; }
    public int TotalMockInterviews { get; set; }
    public int PassedInterviews { get; set; }
    public int ActiveWeakAreas { get; set; }
    public int TotalStudyHours { get; set; }
    public double AverageInterviewScore { get; set; }
    public double DSACompletionRate { get; set; }
    public double SystemDesignProgress { get; set; }
}

public class DSAAnalytics
{
    public Dictionary<string, int> ProblemsByCategory { get; set; } = new();
    public Dictionary<string, int> ProblemsByDifficulty { get; set; } = new();
    public Dictionary<string, int> ProblemsByStatus { get; set; } = new();
    public List<CategoryPerformance> CategoryPerformance { get; set; } = new();
    public List<DSAProblem> NeedsReview { get; set; } = new();
    public double AverageTimePerProblem { get; set; }
    public double OptimalSolutionRate { get; set; }
}

public class CategoryPerformance
{
    public string Category { get; set; } = string.Empty;
    public int TotalProblems { get; set; }
    public int Solved { get; set; }
    public double SuccessRate { get; set; }
    public double AverageTime { get; set; }
    public string StrengthLevel { get; set; } = string.Empty; // Weak, Average, Strong
}

public class SystemDesignAnalytics
{
    public Dictionary<string, int> TopicsByCategory { get; set; } = new();
    public Dictionary<string, int> TopicsByStatus { get; set; } = new();
    public List<TopicProgress> TopicProgress { get; set; } = new();
    public double AverageConfidence { get; set; }
}

public class TopicProgress
{
    public string Category { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Mastered { get; set; }
    public double Progress { get; set; }
}

public class InterviewAnalytics
{
    public Dictionary<string, double> AverageScoresByType { get; set; } = new();
    public List<ScoreTrend> ScoreTrends { get; set; } = new();
    public double OverallPassRate { get; set; }
    public double AverageCommunicationScore { get; set; }
    public double AverageProblemSolvingScore { get; set; }
    public double AverageTechnicalScore { get; set; }
    public List<string> CommonWeaknesses { get; set; } = new();
}

public class ScoreTrend
{
    public DateTime Date { get; set; }
    public double Score { get; set; }
    public string Type { get; set; } = string.Empty;
}

public class WeakAreaAnalytics
{
    public List<WeakAreaSummary> ActiveWeakAreas { get; set; } = new();
    public Dictionary<string, int> WeakAreasByCategory { get; set; } = new();
    public int ResolvedThisMonth { get; set; }
    public List<string> RecommendedFocusAreas { get; set; } = new();
}

public class WeakAreaSummary
{
    public string Area { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public int DaysIdentified { get; set; }
}

public class StudyAnalytics
{
    public int TotalHoursThisWeek { get; set; }
    public int TotalHoursThisMonth { get; set; }
    public Dictionary<string, int> HoursByType { get; set; } = new();
    public List<DailyStudy> DailyStudyData { get; set; } = new();
    public double AverageProductivity { get; set; }
}

public class DailyStudy
{
    public DateTime Date { get; set; }
    public int Minutes { get; set; }
    public string Type { get; set; } = string.Empty;
}

public class DSAProblem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime? NextReviewDate { get; set; }
}
