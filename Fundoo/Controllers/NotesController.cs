using System.Security.Claims;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace Fundoo.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly INoteBL _noteBL;
    public NotesController(INoteBL noteBL) => _noteBL = noteBL;

    private bool TryGetUserId(out int userId)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("UserId")?.Value;
        return int.TryParse(claim, out userId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] NoteCreationModel model)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var note = await _noteBL.CreateNote(userId, model);
        return StatusCode(201, new { success = true, message = "Note created", data = note });
    }

    [HttpGet]
    public async Task<IActionResult> ViewNotes()
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        return Ok(new { success = true, data = await _noteBL.ViewNotes(userId) });
    }

    [HttpGet("by-id")]
    public async Task<IActionResult> ViewNotesById([FromQuery] NotesIdModel model)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var note = await _noteBL.ViewNotesById(userId, model);
        return note == null ? NotFound(new { success = false, message = "Note not found" })
                            : Ok(new { success = true, data = note });
    }

    [HttpPut]
    public async Task<IActionResult> EditNotes([FromBody] EditNotesModel model)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var note = await _noteBL.EditNotes(userId, model);
        return note == null ? NotFound(new { success = false, message = "Note not found" })
                            : Ok(new { success = true, message = "Note updated", data = note });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteNotes([FromQuery] NotesIdModel model)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var deleted = await _noteBL.DeleteNotes(userId, model);
        return deleted ? Ok(new { success = true, message = "Note deleted" })
                       : NotFound(new { success = false, message = "Note not found" });
    }

    [HttpPatch("archive")]
    public async Task<IActionResult> ArchiUnarchived([FromBody] NotesIdModel model)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var note = await _noteBL.ArchiUnarchived(userId, model);
        return note == null ? NotFound(new { success = false, message = "Note not found" })
                            : Ok(new { success = true, data = note });
    }

    [HttpPatch("trash")]
    public async Task<IActionResult> TrashUntrash([FromBody] NotesIdModel model)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var note = await _noteBL.TrashUntrash(userId, model);
        return note == null ? NotFound(new { success = false, message = "Note not found" })
                            : Ok(new { success = true, data = note });
    }
}