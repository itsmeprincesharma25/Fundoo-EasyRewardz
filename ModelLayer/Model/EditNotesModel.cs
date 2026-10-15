using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model;

public class EditNotesModel
{
    [Required]
    public int NoteId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Colour { get; set; }
}