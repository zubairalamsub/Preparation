using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.Models;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeakAreaController : ControllerBase
{
    private readonly AppDbContext _context;

    public WeakAreaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeakArea>>> GetAll([FromQuery] bool? resolved = null)
    {
        var query = _context.WeakAreas.AsQueryable();

        if (resolved.HasValue)
            query = query.Where(w => w.IsResolved == resolved.Value);

        return await query.OrderByDescending(w => w.Severity == "High")
            .ThenByDescending(w => w.Severity == "Medium")
            .ThenByDescending(w => w.IdentifiedAt)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WeakArea>> GetById(int id)
    {
        var weakArea = await _context.WeakAreas.FindAsync(id);
        if (weakArea == null) return NotFound();
        return weakArea;
    }

    [HttpPost]
    public async Task<ActionResult<WeakArea>> Create(WeakArea weakArea)
    {
        weakArea.IdentifiedAt = DateTime.UtcNow;
        _context.WeakAreas.Add(weakArea);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = weakArea.Id }, weakArea);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, WeakArea weakArea)
    {
        if (id != weakArea.Id) return BadRequest();

        _context.Entry(weakArea).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.WeakAreas.AnyAsync(w => w.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpPost("{id}/resolve")]
    public async Task<ActionResult<WeakArea>> Resolve(int id)
    {
        var weakArea = await _context.WeakAreas.FindAsync(id);
        if (weakArea == null) return NotFound();

        weakArea.IsResolved = true;
        weakArea.ResolvedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return weakArea;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var weakArea = await _context.WeakAreas.FindAsync(id);
        if (weakArea == null) return NotFound();

        _context.WeakAreas.Remove(weakArea);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
