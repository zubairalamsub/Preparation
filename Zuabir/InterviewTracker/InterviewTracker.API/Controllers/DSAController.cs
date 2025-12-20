using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.Models;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DSAController : ControllerBase
{
    private readonly AppDbContext _context;

    public DSAController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DSAProblem>>> GetAll(
        [FromQuery] string? category = null,
        [FromQuery] string? difficulty = null,
        [FromQuery] string? status = null,
        [FromQuery] bool? favorite = null)
    {
        var query = _context.DSAProblems.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);
        if (!string.IsNullOrEmpty(difficulty))
            query = query.Where(p => p.Difficulty == difficulty);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);
        if (favorite.HasValue)
            query = query.Where(p => p.IsFavorite == favorite.Value);

        return await query.OrderByDescending(p => p.IsFavorite).ThenByDescending(p => p.LastAttemptedAt ?? p.CreatedAt).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DSAProblem>> GetById(int id)
    {
        var problem = await _context.DSAProblems.FindAsync(id);
        if (problem == null) return NotFound();
        return problem;
    }

    [HttpPost]
    public async Task<ActionResult<DSAProblem>> Create(DSAProblem problem)
    {
        problem.CreatedAt = DateTime.UtcNow;
        _context.DSAProblems.Add(problem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = problem.Id }, problem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DSAProblem problem)
    {
        if (id != problem.Id) return BadRequest();

        problem.LastAttemptedAt = DateTime.UtcNow;
        _context.Entry(problem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.DSAProblems.AnyAsync(p => p.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var problem = await _context.DSAProblems.FindAsync(id);
        if (problem == null) return NotFound();

        _context.DSAProblems.Remove(problem);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/attempt")]
    public async Task<ActionResult<DSAProblem>> RecordAttempt(int id, [FromBody] AttemptRequest request)
    {
        var problem = await _context.DSAProblems.FindAsync(id);
        if (problem == null) return NotFound();

        problem.AttemptCount++;
        problem.LastAttemptedAt = DateTime.UtcNow;
        problem.TimeTakenMinutes = request.TimeTakenMinutes;
        problem.SolvedOptimally = request.SolvedOptimally;
        problem.Status = request.Status;
        if (!string.IsNullOrEmpty(request.Notes))
            problem.Notes = request.Notes;

        // Set next review date based on spaced repetition
        problem.NextReviewDate = CalculateNextReview(problem.AttemptCount, request.SolvedOptimally);

        await _context.SaveChangesAsync();
        return problem;
    }

    private DateTime CalculateNextReview(int attemptCount, bool solvedOptimally)
    {
        // Simple spaced repetition: 1, 3, 7, 14, 30 days
        int[] intervals = { 1, 3, 7, 14, 30 };
        int index = Math.Min(attemptCount - 1, intervals.Length - 1);
        int days = solvedOptimally ? intervals[index] : Math.Max(1, intervals[index] / 2);
        return DateTime.UtcNow.AddDays(days);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        return await _context.DSAProblems
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    [HttpGet("needs-review")]
    public async Task<ActionResult<IEnumerable<DSAProblem>>> GetNeedsReview()
    {
        return await _context.DSAProblems
            .Where(p => p.NextReviewDate != null && p.NextReviewDate <= DateTime.UtcNow)
            .OrderBy(p => p.NextReviewDate)
            .ToListAsync();
    }

    [HttpPost("{id}/favorite")]
    public async Task<ActionResult<DSAProblem>> ToggleFavorite(int id)
    {
        var problem = await _context.DSAProblems.FindAsync(id);
        if (problem == null) return NotFound();

        problem.IsFavorite = !problem.IsFavorite;
        await _context.SaveChangesAsync();
        return problem;
    }

    [HttpPost("seed")]
    public async Task<ActionResult> Seed()
    {
        if (await _context.DSAProblems.AnyAsync())
            return BadRequest("Data already exists. Clear the database first.");

        var problems = Data.SeedData.GetDSAProblems();
        _context.DSAProblems.AddRange(problems);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Seeded {problems.Count} DSA problems" });
    }

    [HttpGet("favorites")]
    public async Task<ActionResult<IEnumerable<DSAProblem>>> GetFavorites()
    {
        return await _context.DSAProblems
            .Where(p => p.IsFavorite)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Title)
            .ToListAsync();
    }
}

public class AttemptRequest
{
    public int TimeTakenMinutes { get; set; }
    public bool SolvedOptimally { get; set; }
    public string Status { get; set; } = "Solved";
    public string? Notes { get; set; }
}
