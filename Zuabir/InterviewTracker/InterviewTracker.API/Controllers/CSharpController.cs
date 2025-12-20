using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.Models;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CSharpController : ControllerBase
{
    private readonly AppDbContext _context;

    public CSharpController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CSharpTopic>>> GetAll(
        [FromQuery] string? category = null,
        [FromQuery] string? status = null)
    {
        var query = _context.CSharpTopics.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(t => t.Category == category);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status == status);

        return await query.OrderByDescending(t => t.IsFavorite).ThenBy(t => t.Category).ThenBy(t => t.Title).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CSharpTopic>> GetById(int id)
    {
        var topic = await _context.CSharpTopics.FindAsync(id);
        if (topic == null) return NotFound();
        return topic;
    }

    [HttpPost]
    public async Task<ActionResult<CSharpTopic>> Create(CSharpTopic topic)
    {
        topic.CreatedAt = DateTime.UtcNow;
        _context.CSharpTopics.Add(topic);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = topic.Id }, topic);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CSharpTopic topic)
    {
        if (id != topic.Id) return BadRequest();

        topic.LastReviewedAt = DateTime.UtcNow;
        _context.Entry(topic).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.CSharpTopics.AnyAsync(t => t.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var topic = await _context.CSharpTopics.FindAsync(id);
        if (topic == null) return NotFound();

        _context.CSharpTopics.Remove(topic);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/favorite")]
    public async Task<ActionResult<CSharpTopic>> ToggleFavorite(int id)
    {
        var topic = await _context.CSharpTopics.FindAsync(id);
        if (topic == null) return NotFound();

        topic.IsFavorite = !topic.IsFavorite;
        await _context.SaveChangesAsync();
        return topic;
    }

    [HttpDelete("clear")]
    public async Task<ActionResult> Clear()
    {
        var topics = await _context.CSharpTopics.ToListAsync();
        _context.CSharpTopics.RemoveRange(topics);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Cleared {topics.Count} C# topics" });
    }

    [HttpPost("seed")]
    public async Task<ActionResult> Seed()
    {
        // Clear existing data first
        var existing = await _context.CSharpTopics.ToListAsync();
        if (existing.Any())
        {
            _context.CSharpTopics.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }

        var topics = SeedDataWithLessons.GetCSharpTopics();
        _context.CSharpTopics.AddRange(topics);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Seeded {topics.Count} C# topics with detailed lessons" });
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        return await _context.CSharpTopics
            .Select(t => t.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }
}
