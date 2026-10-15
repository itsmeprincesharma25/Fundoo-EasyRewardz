using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service;

public class NoteBL : INoteBL
{
    private readonly INoteRL _noteRL;
    public NoteBL(INoteRL noteRL) => _noteRL = noteRL;

    public Task<NoteEntity> CreateNote(int userId, NoteCreationModel model) =>
        _noteRL.CreateNote(new NoteEntity
        {
            Title = model.Title,
            Description = model.Description,
            Colour = model.Colour,
            UserId = userId
        });

    public Task<List<NoteEntity>> ViewNotes(int userId) => _noteRL.GetNotes(userId);

    public Task<NoteEntity?> ViewNotesById(int userId, NotesIdModel model) =>
        _noteRL.GetNoteById(model.NoteId, userId);

    public async Task<NoteEntity?> EditNotes(int userId, EditNotesModel model)
    {
        var note = await _noteRL.GetNoteById(model.NoteId, userId);
        if (note == null) return null;

        note.Title = model.Title;
        note.Description = model.Description;
        note.Colour = model.Colour;
        await _noteRL.UpdateNote(note);
        return note;
    }

    public async Task<bool> DeleteNotes(int userId, NotesIdModel model)
    {
        var note = await _noteRL.GetNoteById(model.NoteId, userId);
        if (note == null) return false;

        await _noteRL.DeleteNote(note);
        return true;
    }

    public async Task<NoteEntity?> ArchiUnarchived(int userId, NotesIdModel model)
    {
        var note = await _noteRL.GetNoteById(model.NoteId, userId);
        if (note == null) return null;

        note.IsArchived = !note.IsArchived;
        await _noteRL.UpdateNote(note);
        return note;
    }

    public async Task<NoteEntity?> TrashUntrash(int userId, NotesIdModel model)
    {
        var note = await _noteRL.GetNoteById(model.NoteId, userId);
        if (note == null) return null;

        note.IsDeleted = !note.IsDeleted;
        await _noteRL.UpdateNote(note);
        return note;
    }
}