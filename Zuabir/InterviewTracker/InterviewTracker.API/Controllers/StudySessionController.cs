using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.API.Data;
using InterviewTracker.API.Models;

namespace InterviewTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudySessionController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudySessionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudySession>>> GetAll(
        [FromQuery] string? type = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var query = _context.StudySessions.AsQueryable();

        if (!string.IsNullOrEmpty(type))
            query = query.Where(s => s.Type == type);
        if (from.HasValue)
            query = query.Where(s => s.SessionDate >= from.Value);
        if (to.HasValue)
            query = query.Where(s => s.SessionDate <= to.Value);

        return await query.OrderByDescending(s => s.SessionDate).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudySession>> GetById(int id)
    {
        var session = await _context.StudySessions.FindAsync(id);
        if (session == null) return NotFound();
        return session;
    }

    [HttpPost]
    public async Task<ActionResult<StudySession>> Create(StudySession session)
    {
        _context.StudySessions.Add(session);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, StudySession session)
    {
        if (id != session.Id) return BadRequest();

        _context.Entry(session).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.StudySessions.AnyAsync(s => s.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var session = await _context.StudySessions.FindAsync(id);
        if (session == null) return NotFound();

        _context.StudySessions.Remove(session);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
