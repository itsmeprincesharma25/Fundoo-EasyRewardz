using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface;

public interface INoteBL
{
    Task<NoteEntity> CreateNote(int userId, NoteCreationModel model);
    Task<List<NoteEntity>> ViewNotes(int userId);
    Task<NoteEntity?> ViewNotesById(int userId, NotesIdModel model);
    Task<NoteEntity?> EditNotes(int userId, EditNotesModel model);
    Task<bool> DeleteNotes(int userId, NotesIdModel model);
    Task<NoteEntity?> ArchiUnarchived(int userId, NotesIdModel model);
    Task<NoteEntity?> TrashUntrash(int userId, NotesIdModel model);
}