using System;
using System.Collections.Generic;

namespace MiMangaBot.Data.ScaffoldedModels;

public partial class Genero1
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Manga> Mangas { get; set; } = new List<Manga>();
}
