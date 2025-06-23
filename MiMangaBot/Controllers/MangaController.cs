using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Data.ScaffoldedModels;
using MiMangaBot.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace MiMangaBot.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MangaController : ControllerBase
{
    private readonly IMangaRepository _mangaRepository;
    private readonly ILogger<MangaController> _logger;

    public MangaController(IMangaRepository mangaRepository, ILogger<MangaController> logger)
    {
        _mangaRepository = mangaRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 30)
    {
        try
        {
            _logger.LogInformation($"Obteniendo mangas - Página {page}, Tamaño de página: {pageSize}");
            var allMangas = await _mangaRepository.GetAllAsync(page, pageSize);

            // Paginación
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 30;
            var totalMangas = allMangas.Count;
            var totalPages = (int)Math.Ceiling(totalMangas / (double)pageSize);
            var mangas = allMangas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                totalMangas,
                totalPages,
                currentPage = page,
                pageSize,
                mangas
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los mangas");
            return StatusCode(500, "Error al obtener los mangas: " + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            _logger.LogInformation($"Obteniendo manga con ID: {id}");
            var manga = await _mangaRepository.GetByIdAsync(id);
            
            if (manga == null)
            {
                return NotFound($"Manga con ID {id} no encontrado");
            }

            return Ok(manga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener manga con ID: {id}");
            return StatusCode(500, "Error al obtener el manga: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Manga manga)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _mangaRepository.ExistsByTitleAsync(manga.Titulo))
            {
                return BadRequest($"Ya existe un manga con el título: {manga.Titulo}");
            }

            _logger.LogInformation($"Creando nuevo manga: {manga.Titulo}");
            manga.Fechacreacion = DateTime.UtcNow;
            var createdManga = await _mangaRepository.AddAsync(manga);
            
            return CreatedAtAction(nameof(GetById), new { id = createdManga.Id }, createdManga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear manga");
            return StatusCode(500, "Error al crear el manga: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Manga manga)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingManga = await _mangaRepository.GetByIdAsync(id);
            if (existingManga == null)
            {
                return NotFound($"Manga con ID {id} no encontrado");
            }

            // Verificar si el nuevo título ya existe (si se está cambiando)
            if (manga.Titulo != existingManga.Titulo && 
                await _mangaRepository.ExistsByTitleAsync(manga.Titulo))
            {
                return BadRequest($"Ya existe un manga con el título: {manga.Titulo}");
            }

            manga.Id = id; // Asegurar que el ID no cambie
            manga.Fechaactualizacion = DateTime.UtcNow;
            
            _logger.LogInformation($"Actualizando manga con ID: {id}");
            var success = await _mangaRepository.UpdateAsync(manga);
            
            if (!success)
            {
                return StatusCode(500, "Error al actualizar el manga");
            }

            return Ok(manga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar manga con ID: {id}");
            return StatusCode(500, "Error al actualizar el manga: " + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var existingManga = await _mangaRepository.GetByIdAsync(id);
            if (existingManga == null)
            {
                return NotFound($"Manga con ID {id} no encontrado");
            }

            _logger.LogInformation($"Eliminando manga con ID: {id}");
            var success = await _mangaRepository.DeleteAsync(id);
            
            if (!success)
            {
                return StatusCode(500, "Error al eliminar el manga");
            }

            return Ok($"Manga con ID {id} eliminado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar manga con ID: {id}");
            return StatusCode(500, "Error al eliminar el manga: " + ex.Message);
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? titulo = null, [FromQuery] string? genero = null)
    {
        try
        {
            _logger.LogInformation("Buscando mangas con filtros");
            var allMangas = await _mangaRepository.GetAllAsync();
            
            var filteredMangas = allMangas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(titulo))
            {
                filteredMangas = filteredMangas.Where(m => 
                    m.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(genero))
            {
                filteredMangas = filteredMangas.Where(m => 
                    m.Genero != null && m.Genero.Nombre.Equals(genero, StringComparison.OrdinalIgnoreCase));
            }

            var result = filteredMangas.ToList();
            return Ok(new { count = result.Count, mangas = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar mangas");
            return StatusCode(500, "Error al buscar mangas: " + ex.Message);
        }
    }
} 