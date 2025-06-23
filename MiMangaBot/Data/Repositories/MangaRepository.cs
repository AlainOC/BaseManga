using MiMangaBot.Domain.Models;
using MiMangaBot.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MiMangaBot.Data.ScaffoldedModels;

namespace MiMangaBot.Data.Repositories
{
    public class MangaRepository : IMangaRepository
    {
        private readonly RailwayContext _context;

        public MangaRepository(RailwayContext context)
        {
            _context = context;
        }

        public async Task<List<Manga>> GetAllAsync(int page = 1, int pageSize = 30)
        {
            return await _context.Mangas
                .Include(m => m.Genero)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Manga?> GetByIdAsync(Guid id)
        {
            return await _context.Mangas.Include(m => m.Genero).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Manga> AddAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
            return manga;
        }

        public async Task<bool> UpdateAsync(Manga manga)
        {
            _context.Mangas.Update(manga);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var manga = await _context.Mangas.FindAsync(id);
            if (manga == null) return false;
            _context.Mangas.Remove(manga);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.Mangas.AnyAsync(m => m.Titulo.ToLower() == title.ToLower());
        }

        public async Task<Dictionary<string, List<Manga>>> GetDuplicatesAsync()
        {
            var allMangas = await GetAllAsync();
            return allMangas
                .GroupBy(m => m.TituloNormalizado())
                .Where(g => g.Count() > 1)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public async Task<List<Manga>> AddRangeAsync(IEnumerable<Manga> mangas)
        {
            _context.Mangas.AddRange(mangas);
            await _context.SaveChangesAsync();
            return mangas.ToList();
        }
    }

    public static class MangaExtensions
    {
        public static string TituloNormalizado(this Manga manga)
        {
            return manga.Titulo.ToLower().Trim();
        }
    }
} 