using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.Models;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InterviewController : ControllerBase
{
    private readonly AppDbContext _context;

    public InterviewController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MockInterview>>> GetAll(
        [FromQuery] string? type = null,
        [FromQuery] string? company = null)
    {
        var query = _context.MockInterviews.AsQueryable();

        if (!string.IsNullOrEmpty(type))
            query = query.Where(i => i.Type == type);
        if (!string.IsNullOrEmpty(company))
            query = query.Where(i => i.Company.Contains(company));

        return await query.OrderByDescending(i => i.InterviewDate).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MockInterview>> GetById(int id)
    {
        var interview = await _context.MockInterviews.FindAsync(id);
        if (interview == null) return NotFound();
        return interview;
    }

    [HttpPost]
    public async Task<ActionResult<MockInterview>> Create(MockInterview interview)
    {
        interview.CreatedAt = DateTime.UtcNow;
        _context.MockInterviews.Add(interview);
        await _context.SaveChangesAsync();

        // Auto-identify weak areas from feedback
        await AnalyzeAndCreateWeakAreas(interview);

        return CreatedAtAction(nameof(GetById), new { id = interview.Id }, interview);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, MockInterview interview)
    {
        if (id != interview.Id) return BadRequest();

        _context.Entry(interview).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.MockInterviews.AnyAsync(i => i.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var interview = await _context.MockInterviews.FindAsync(id);
        if (interview == null) return NotFound();

        _context.MockInterviews.Remove(interview);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task AnalyzeAndCreateWeakAreas(MockInterview interview)
    {
        // Create weak areas based on low scores
        if (interview.CommunicationScore < 6)
        {
            await CreateWeakAreaIfNotExists("Communication Skills", "Behavioral",
                interview.CommunicationScore < 4 ? "High" : "Medium");
        }
        if (interview.ProblemSolvingScore < 6)
        {
            await CreateWeakAreaIfNotExists("Problem Solving Approach", interview.Type,
                interview.ProblemSolvingScore < 4 ? "High" : "Medium");
        }
        if (interview.TechnicalScore < 6)
        {
            await CreateWeakAreaIfNotExists("Technical Knowledge", interview.Type,
                interview.TechnicalScore < 4 ? "High" : "Medium");
        }
    }

    private async Task CreateWeakAreaIfNotExists(string area, string category, string severity)
    {
        var exists = await _context.WeakAreas
            .AnyAsync(w => w.Area == area && w.Category == category && !w.IsResolved);

        if (!exists)
        {
            _context.WeakAreas.Add(new WeakArea
            {
                Area = area,
                Category = category,
                Severity = severity,
                IdentifiedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }
}
