using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.Models;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AzureController : ControllerBase
{
    private readonly AppDbContext _context;

    public AzureController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AzureTopic>>> GetAll(
        [FromQuery] string? category = null,
        [FromQuery] string? status = null)
    {
        var query = _context.AzureTopics.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(t => t.Category == category);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status == status);

        return await query.OrderByDescending(t => t.IsFavorite).ThenBy(t => t.Category).ThenBy(t => t.Title).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AzureTopic>> GetById(int id)
    {
        var topic = await _context.AzureTopics.FindAsync(id);
        if (topic == null) return NotFound();
        return topic;
    }

    [HttpPost]
    public async Task<ActionResult<AzureTopic>> Create(AzureTopic topic)
    {
        topic.CreatedAt = DateTime.UtcNow;
        _context.AzureTopics.Add(topic);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = topic.Id }, topic);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AzureTopic topic)
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
            if (!await _context.AzureTopics.AnyAsync(t => t.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var topic = await _context.AzureTopics.FindAsync(id);
        if (topic == null) return NotFound();

        _context.AzureTopics.Remove(topic);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/favorite")]
    public async Task<ActionResult<AzureTopic>> ToggleFavorite(int id)
    {
        var topic = await _context.AzureTopics.FindAsync(id);
        if (topic == null) return NotFound();

        topic.IsFavorite = !topic.IsFavorite;
        await _context.SaveChangesAsync();
        return topic;
    }

    [HttpDelete("clear")]
    public async Task<ActionResult> Clear()
    {
        var topics = await _context.AzureTopics.ToListAsync();
        _context.AzureTopics.RemoveRange(topics);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Cleared {topics.Count} Azure topics" });
    }

    [HttpPost("seed")]
    public async Task<ActionResult> Seed()
    {
        var existing = await _context.AzureTopics.ToListAsync();
        if (existing.Any())
        {
            _context.AzureTopics.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }

        var topics = SeedDataWithLessons.GetAzureTopics();
        _context.AzureTopics.AddRange(topics);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Seeded {topics.Count} Azure topics with detailed lessons" });
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        return await _context.AzureTopics
            .Select(t => t.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }
}
