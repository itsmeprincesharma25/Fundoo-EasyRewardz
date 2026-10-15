using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service;

public class NoteRL : INoteRL
{
    private readonly FundooContext _context;
    public NoteRL(FundooContext context) => _context = context;

    public async Task<NoteEntity> CreateNote(NoteEntity note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
        return note;
    }

    public Task<List<NoteEntity>> GetNotes(int userId) =>
        _context.Notes.AsNoTracking().Where(n => n.UserId == userId).ToListAsync();

    public Task<NoteEntity?> GetNoteById(int noteId, int userId) =>
        _context.Notes.FirstOrDefaultAsync(n => n.NoteId == noteId && n.UserId == userId);

    public async Task UpdateNote(NoteEntity note)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNote(NoteEntity note)
    {
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }
}