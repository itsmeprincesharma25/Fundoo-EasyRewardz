using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model;

public class NoteCreationModel
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Colour { get; set; }
}