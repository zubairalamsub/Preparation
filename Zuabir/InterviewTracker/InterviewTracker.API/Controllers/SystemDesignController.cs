using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.Models;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SystemDesignController : ControllerBase
{
    private readonly AppDbContext _context;

    public SystemDesignController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemDesignTopic>>> GetAll(
        [FromQuery] string? category = null,
        [FromQuery] string? status = null,
        [FromQuery] bool? favorite = null)
    {
        var query = _context.SystemDesignTopics.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(t => t.Category == category);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status == status);
        if (favorite.HasValue)
            query = query.Where(t => t.IsFavorite == favorite.Value);

        return await query.OrderByDescending(t => t.IsFavorite).ThenByDescending(t => t.LastReviewedAt ?? t.CreatedAt).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SystemDesignTopic>> GetById(int id)
    {
        var topic = await _context.SystemDesignTopics.FindAsync(id);
        if (topic == null) return NotFound();
        return topic;
    }

    [HttpPost]
    public async Task<ActionResult<SystemDesignTopic>> Create(SystemDesignTopic topic)
    {
        topic.CreatedAt = DateTime.UtcNow;
        _context.SystemDesignTopics.Add(topic);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = topic.Id }, topic);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SystemDesignTopic topic)
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
            if (!await _context.SystemDesignTopics.AnyAsync(t => t.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var topic = await _context.SystemDesignTopics.FindAsync(id);
        if (topic == null) return NotFound();

        _context.SystemDesignTopics.Remove(topic);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/review")]
    public async Task<ActionResult<SystemDesignTopic>> RecordReview(int id, [FromBody] ReviewRequest request)
    {
        var topic = await _context.SystemDesignTopics.FindAsync(id);
        if (topic == null) return NotFound();

        topic.LastReviewedAt = DateTime.UtcNow;
        topic.ConfidenceLevel = request.ConfidenceLevel;
        topic.Status = request.Status;
        if (!string.IsNullOrEmpty(request.Notes))
            topic.Notes = request.Notes;

        await _context.SaveChangesAsync();
        return topic;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        return await _context.SystemDesignTopics
            .Select(t => t.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    [HttpPost("{id}/favorite")]
    public async Task<ActionResult<SystemDesignTopic>> ToggleFavorite(int id)
    {
        var topic = await _context.SystemDesignTopics.FindAsync(id);
        if (topic == null) return NotFound();

        topic.IsFavorite = !topic.IsFavorite;
        await _context.SaveChangesAsync();
        return topic;
    }

    [HttpPost("seed")]
    public async Task<ActionResult> SeedData()
    {
        if (await _context.SystemDesignTopics.AnyAsync())
            return BadRequest("Data already exists. Clear the database first.");

        var topics = Data.SeedData.GetSystemDesignTopics();
        _context.SystemDesignTopics.AddRange(topics);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Seeded {topics.Count} System Design topics" });
    }

    [HttpGet("favorites")]
    public async Task<ActionResult<IEnumerable<SystemDesignTopic>>> GetFavorites()
    {
        return await _context.SystemDesignTopics
            .Where(t => t.IsFavorite)
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Title)
            .ToListAsync();
    }
}

public class ReviewRequest
{
    public int ConfidenceLevel { get; set; }
    public string Status { get; set; } = "Learning";
    public string? Notes { get; set; }
}
