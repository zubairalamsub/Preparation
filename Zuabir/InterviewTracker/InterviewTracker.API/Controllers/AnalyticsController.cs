using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.DTOs;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnalyticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardStats>> GetDashboardStats()
    {
        var dsaProblems = await _context.DSAProblems.ToListAsync();
        var systemDesign = await _context.SystemDesignTopics.ToListAsync();
        var interviews = await _context.MockInterviews.ToListAsync();
        var weakAreas = await _context.WeakAreas.ToListAsync();
        var studySessions = await _context.StudySessions.ToListAsync();

        return new DashboardStats
        {
            TotalDSAProblems = dsaProblems.Count,
            SolvedDSAProblems = dsaProblems.Count(p => p.Status == "Solved"),
            TotalSystemDesignTopics = systemDesign.Count,
            MasteredTopics = systemDesign.Count(t => t.Status == "Mastered"),
            TotalMockInterviews = interviews.Count,
            PassedInterviews = interviews.Count(i => i.Passed),
            ActiveWeakAreas = weakAreas.Count(w => !w.IsResolved),
            TotalStudyHours = studySessions.Sum(s => s.DurationMinutes) / 60,
            AverageInterviewScore = interviews.Any() ? interviews.Average(i => i.OverallScore) : 0,
            DSACompletionRate = dsaProblems.Any() ? (double)dsaProblems.Count(p => p.Status == "Solved") / dsaProblems.Count * 100 : 0,
            SystemDesignProgress = systemDesign.Any() ? (double)systemDesign.Count(t => t.Status == "Mastered" || t.Status == "Understood") / systemDesign.Count * 100 : 0
        };
    }

    [HttpGet("dsa")]
    public async Task<ActionResult<DSAAnalytics>> GetDSAAnalytics()
    {
        var problems = await _context.DSAProblems.ToListAsync();

        var categoryPerformance = problems
            .GroupBy(p => p.Category)
            .Select(g => new CategoryPerformance
            {
                Category = g.Key,
                TotalProblems = g.Count(),
                Solved = g.Count(p => p.Status == "Solved"),
                SuccessRate = g.Any() ? (double)g.Count(p => p.Status == "Solved") / g.Count() * 100 : 0,
                AverageTime = g.Any() ? g.Average(p => p.TimeTakenMinutes) : 0,
                StrengthLevel = GetStrengthLevel((double)g.Count(p => p.Status == "Solved") / g.Count() * 100)
            })
            .OrderBy(c => c.SuccessRate)
            .ToList();

        return new DSAAnalytics
        {
            ProblemsByCategory = problems.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.Count()),
            ProblemsByDifficulty = problems.GroupBy(p => p.Difficulty).ToDictionary(g => g.Key, g => g.Count()),
            ProblemsByStatus = problems.GroupBy(p => p.Status).ToDictionary(g => g.Key, g => g.Count()),
            CategoryPerformance = categoryPerformance,
            NeedsReview = problems
                .Where(p => p.NextReviewDate != null && p.NextReviewDate <= DateTime.UtcNow)
                .Select(p => new DTOs.DSAProblem { Id = p.Id, Title = p.Title, Category = p.Category, NextReviewDate = p.NextReviewDate })
                .ToList(),
            AverageTimePerProblem = problems.Any() ? problems.Average(p => p.TimeTakenMinutes) : 0,
            OptimalSolutionRate = problems.Any() ? (double)problems.Count(p => p.SolvedOptimally) / problems.Count * 100 : 0
        };
    }

    [HttpGet("system-design")]
    public async Task<ActionResult<SystemDesignAnalytics>> GetSystemDesignAnalytics()
    {
        var topics = await _context.SystemDesignTopics.ToListAsync();

        var topicProgress = topics
            .GroupBy(t => t.Category)
            .Select(g => new TopicProgress
            {
                Category = g.Key,
                Total = g.Count(),
                Mastered = g.Count(t => t.Status == "Mastered"),
                Progress = g.Any() ? (double)g.Count(t => t.Status == "Mastered" || t.Status == "Understood") / g.Count() * 100 : 0
            })
            .ToList();

        return new SystemDesignAnalytics
        {
            TopicsByCategory = topics.GroupBy(t => t.Category).ToDictionary(g => g.Key, g => g.Count()),
            TopicsByStatus = topics.GroupBy(t => t.Status).ToDictionary(g => g.Key, g => g.Count()),
            TopicProgress = topicProgress,
            AverageConfidence = topics.Any() ? topics.Average(t => t.ConfidenceLevel) : 0
        };
    }

    [HttpGet("interviews")]
    public async Task<ActionResult<InterviewAnalytics>> GetInterviewAnalytics()
    {
        var interviews = await _context.MockInterviews.OrderBy(i => i.InterviewDate).ToListAsync();

        var scoreTrends = interviews.Select(i => new ScoreTrend
        {
            Date = i.InterviewDate,
            Score = i.OverallScore,
            Type = i.Type
        }).ToList();

        var commonWeaknesses = new List<string>();
        if (interviews.Average(i => i.CommunicationScore) < 7)
            commonWeaknesses.Add("Communication");
        if (interviews.Average(i => i.ProblemSolvingScore) < 7)
            commonWeaknesses.Add("Problem Solving");
        if (interviews.Average(i => i.TechnicalScore) < 7)
            commonWeaknesses.Add("Technical Skills");

        return new InterviewAnalytics
        {
            AverageScoresByType = interviews
                .GroupBy(i => i.Type)
                .ToDictionary(g => g.Key, g => g.Average(i => i.OverallScore)),
            ScoreTrends = scoreTrends,
            OverallPassRate = interviews.Any() ? (double)interviews.Count(i => i.Passed) / interviews.Count * 100 : 0,
            AverageCommunicationScore = interviews.Any() ? interviews.Average(i => i.CommunicationScore) : 0,
            AverageProblemSolvingScore = interviews.Any() ? interviews.Average(i => i.ProblemSolvingScore) : 0,
            AverageTechnicalScore = interviews.Any() ? interviews.Average(i => i.TechnicalScore) : 0,
            CommonWeaknesses = commonWeaknesses
        };
    }

    [HttpGet("weak-areas")]
    public async Task<ActionResult<WeakAreaAnalytics>> GetWeakAreaAnalytics()
    {
        var weakAreas = await _context.WeakAreas.ToListAsync();
        var thisMonth = DateTime.UtcNow.AddDays(-30);

        var activeWeakAreas = weakAreas
            .Where(w => !w.IsResolved)
            .Select(w => new WeakAreaSummary
            {
                Area = w.Area,
                Category = w.Category,
                Severity = w.Severity,
                DaysIdentified = (int)(DateTime.UtcNow - w.IdentifiedAt).TotalDays
            })
            .ToList();

        // Get recommended focus areas based on DSA performance
        var dsaProblems = await _context.DSAProblems.ToListAsync();
        var weakCategories = dsaProblems
            .GroupBy(p => p.Category)
            .Where(g => (double)g.Count(p => p.Status == "Solved") / g.Count() < 0.5)
            .Select(g => g.Key)
            .Take(3)
            .ToList();

        return new WeakAreaAnalytics
        {
            ActiveWeakAreas = activeWeakAreas,
            WeakAreasByCategory = weakAreas
                .Where(w => !w.IsResolved)
                .GroupBy(w => w.Category)
                .ToDictionary(g => g.Key, g => g.Count()),
            ResolvedThisMonth = weakAreas.Count(w => w.IsResolved && w.ResolvedAt >= thisMonth),
            RecommendedFocusAreas = weakCategories
        };
    }

    [HttpGet("study")]
    public async Task<ActionResult<StudyAnalytics>> GetStudyAnalytics()
    {
        var sessions = await _context.StudySessions.ToListAsync();
        var thisWeek = DateTime.UtcNow.AddDays(-7);
        var thisMonth = DateTime.UtcNow.AddDays(-30);

        var dailyData = sessions
            .Where(s => s.SessionDate >= thisMonth)
            .GroupBy(s => s.SessionDate.Date)
            .Select(g => new DailyStudy
            {
                Date = g.Key,
                Minutes = g.Sum(s => s.DurationMinutes),
                Type = g.First().Type
            })
            .OrderBy(d => d.Date)
            .ToList();

        return new StudyAnalytics
        {
            TotalHoursThisWeek = sessions.Where(s => s.SessionDate >= thisWeek).Sum(s => s.DurationMinutes) / 60,
            TotalHoursThisMonth = sessions.Where(s => s.SessionDate >= thisMonth).Sum(s => s.DurationMinutes) / 60,
            HoursByType = sessions
                .GroupBy(s => s.Type)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.DurationMinutes) / 60),
            DailyStudyData = dailyData,
            AverageProductivity = sessions.Any() ? sessions.Average(s => s.ProductivityScore) : 0
        };
    }

    private string GetStrengthLevel(double successRate)
    {
        return successRate switch
        {
            >= 70 => "Strong",
            >= 40 => "Average",
            _ => "Weak"
        };
    }
}
