using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface;

public interface INoteRL
{
    Task<NoteEntity> CreateNote(NoteEntity note);
    Task<List<NoteEntity>> GetNotes(int userId);
    Task<NoteEntity?> GetNoteById(int noteId, int userId);
    Task UpdateNote(NoteEntity note);
    Task DeleteNote(NoteEntity note);
}