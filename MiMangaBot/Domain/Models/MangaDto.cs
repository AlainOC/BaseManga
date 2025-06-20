namespace MiMangaBot.Domain.Models;

public class MangaDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public int GeneroId { get; set; }
    public string Genero { get; set; } = null!;
    // Agrega otros campos que quieras exponer
} 