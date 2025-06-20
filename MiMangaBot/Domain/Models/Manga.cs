using System.ComponentModel.DataAnnotations;

namespace MiMangaBot.Domain.Models;

public class Manga
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required(ErrorMessage = "El título es requerido")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El autor es requerido")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El autor debe tener entre 1 y 100 caracteres")]
    public string Autor { get; set; } = string.Empty;
    
    // [Required(ErrorMessage = "El género es requerido")]
    // [StringLength(50, MinimumLength = 1, ErrorMessage = "El género debe tener entre 1 y 50 caracteres")]
    // public string Genero { get; set; } = string.Empty;
    
    [Range(1900, 2100, ErrorMessage = "El año de publicación debe estar entre 1900 y 2100")]
    public int AnioPublicacion { get; set; }
    
    [Range(1, 1000, ErrorMessage = "El número de volúmenes debe estar entre 1 y 1000")]
    public int Volumenes { get; set; }
    
    public bool EnPublicacion { get; set; }
    
    [Required(ErrorMessage = "La sinopsis es requerida")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "La sinopsis debe tener entre 10 y 2000 caracteres")]
    public string Sinopsis { get; set; } = string.Empty;
    
    [Range(0.0, 10.0, ErrorMessage = "La calificación debe estar entre 0 y 10")]
    public double Calificacion { get; set; }
    
    [Range(1, 10000, ErrorMessage = "El número de capítulos debe estar entre 1 y 10000")]
    public int NumeroCapitulos { get; set; }
    
    [Required(ErrorMessage = "La editorial es requerida")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "La editorial debe tener entre 1 y 100 caracteres")]
    public string Editorial { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El estado es requerido")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "El estado debe tener entre 1 y 50 caracteres")]
    public string Estado { get; set; } = string.Empty; // En emisión, Finalizado, Pausado
    
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaActualizacion { get; set; }
    
    [Range(0.01, 1000000.0, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Range(0, 100000, ErrorMessage = "La cantidad en inventario no puede ser negativa")]
    public int CantidadInventario { get; set; }

    [Required(ErrorMessage = "El tipo de manga es requerido")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "El tipo de manga debe tener entre 1 y 50 caracteres")]
    public string TipoManga { get; set; } = string.Empty;

    public bool InventarioActivo { get; set; } = true;
    
    // Propiedad para verificar unicidad
    public string TituloNormalizado => Titulo.ToLower().Trim();

    public int GeneroId { get; set; }
    public Genero? Genero { get; set; }
} 