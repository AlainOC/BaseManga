using MiMangaBot.Domain.Models;

namespace MiMangaBot.Domain.Repositories;

public interface IMangaRepository
{
    Task<List<Manga>> GetAllAsync();
    Task<Manga?> GetByIdAsync(Guid id);
    Task<Manga> AddAsync(Manga manga);
    Task<bool> UpdateAsync(Manga manga);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByTitleAsync(string title);
    Task<Dictionary<string, List<Manga>>> GetDuplicatesAsync();
    Task<List<Manga>> AddRangeAsync(IEnumerable<Manga> mangas);
} 