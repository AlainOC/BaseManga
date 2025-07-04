using Bogus;
using MiMangaBot.Data.ScaffoldedModels;

namespace MiMangaBot.Services;

public class FakerService
{
    private readonly Faker<Manga> _mangaFaker;

    public FakerService()
    {
        var generos = new[] { "Shonen", "Seinen", "Shojo", "Josei", "Mecha", "Fantasy", "Romance", "Comedy", "Drama", "Action" };
        
        _mangaFaker = new Faker<Manga>()
            .RuleFor(m => m.Titulo, f => string.Join(" ", f.Lorem.Words(3)))
            .RuleFor(m => m.Autor, f => f.Name.FullName())
            .RuleFor(m => m.GeneroId, f => f.Random.Int(1, 10))
            .RuleFor(m => m.Aniopublicacion, f => f.Random.Int(1960, 2024))
            .RuleFor(m => m.Fechacreacion, f => f.Date.Past());
    }

    public IEnumerable<Manga> GenerateMangas(int count)
    {
        return _mangaFaker.Generate(count);
    }
} 