using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.Models;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OOPController : ControllerBase
{
    private readonly AppDbContext _context;

    public OOPController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OOPTopic>>> GetAll(
        [FromQuery] string? category = null,
        [FromQuery] string? status = null)
    {
        var query = _context.OOPTopics.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(t => t.Category == category);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status == status);

        return await query.OrderByDescending(t => t.IsFavorite).ThenBy(t => t.Category).ThenBy(t => t.Title).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OOPTopic>> GetById(int id)
    {
        var topic = await _context.OOPTopics.FindAsync(id);
        if (topic == null) return NotFound();
        return topic;
    }

    [HttpPost]
    public async Task<ActionResult<OOPTopic>> Create(OOPTopic topic)
    {
        topic.CreatedAt = DateTime.UtcNow;
        _context.OOPTopics.Add(topic);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = topic.Id }, topic);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OOPTopic topic)
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
            if (!await _context.OOPTopics.AnyAsync(t => t.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var topic = await _context.OOPTopics.FindAsync(id);
        if (topic == null) return NotFound();

        _context.OOPTopics.Remove(topic);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/favorite")]
    public async Task<ActionResult<OOPTopic>> ToggleFavorite(int id)
    {
        var topic = await _context.OOPTopics.FindAsync(id);
        if (topic == null) return NotFound();

        topic.IsFavorite = !topic.IsFavorite;
        await _context.SaveChangesAsync();
        return topic;
    }

    [HttpDelete("clear")]
    public async Task<ActionResult> Clear()
    {
        var topics = await _context.OOPTopics.ToListAsync();
        _context.OOPTopics.RemoveRange(topics);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Cleared {topics.Count} OOP topics" });
    }

    [HttpPost("seed")]
    public async Task<ActionResult> Seed()
    {
        var existing = await _context.OOPTopics.ToListAsync();
        if (existing.Any())
        {
            _context.OOPTopics.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }

        var topics = SeedDataWithLessons.GetOOPTopics();
        _context.OOPTopics.AddRange(topics);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Seeded {topics.Count} OOP topics with detailed lessons" });
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        return await _context.OOPTopics
            .Select(t => t.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }
}
