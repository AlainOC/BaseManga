using MiMangaBot.Domain.Models;
using MiMangaBot.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MiMangaBot.Data.Repositories
{
    public class MangaRepository : IMangaRepository
    {
        private readonly ApplicationDbContext _context;

        public MangaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Manga>> GetAllAsync()
        {
            return await _context.Mangas.Include(m => m.Genero).ToListAsync();
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
                .GroupBy(m => m.TituloNormalizado)
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
} 