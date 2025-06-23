using System;
using System.Collections.Generic;

namespace MiMangaBot.Data.ScaffoldedModels;

public partial class Manga
{
    public Guid Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public int Aniopublicacion { get; set; }

    public int Volumenes { get; set; }

    public bool Enpublicacion { get; set; }

    public string Sinopsis { get; set; } = null!;

    public double Calificacion { get; set; }

    public int Numerocapitulos { get; set; }

    public string Editorial { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateTime Fechacreacion { get; set; }

    public DateTime? Fechaactualizacion { get; set; }

    public decimal Precio { get; set; }

    public int Cantidadinventario { get; set; }

    public string Tipomanga { get; set; } = null!;

    public bool Inventarioactivo { get; set; }

    public int? GeneroId { get; set; }

    public virtual Genero? Genero { get; set; }
}
