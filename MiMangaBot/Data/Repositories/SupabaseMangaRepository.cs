using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;
using MiMangaBot.Domain.Models;
using MiMangaBot.Domain.Repositories;
using System.Text.Json;

namespace MiMangaBot.Infrastructure.Repositories;

public class SupabaseMangaRepository : IMangaRepository
{
    private readonly string _connectionString;

    public SupabaseMangaRepository(IConfiguration configuration)
    {
        _connectionString = configuration["Supabase:ConnectionString"];
        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new InvalidOperationException("La cadena de conexión de Supabase no está configurada.");
    }


    public async Task<List<Manga>> GetAllAsync()
    {
        var mangas = new List<Manga>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand("SELECT m.*, g.nombre as genero_nombre FROM mangas m JOIN generos g ON m.genero_id = g.id", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            mangas.Add(new Manga
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                Titulo = reader["titulo"].ToString()!,
                Autor = reader["autor"].ToString()!,
                GeneroId = reader.GetInt32(reader.GetOrdinal("genero_id")),
                Genero = new Genero
                {
                    Id = reader.GetInt32(reader.GetOrdinal("genero_id")),
                    Nombre = reader["genero_nombre"].ToString()!
                },
                AnioPublicacion = reader.GetInt32(reader.GetOrdinal("aniopublicacion")),
                Volumenes = reader.GetInt32(reader.GetOrdinal("volumenes")),
                EnPublicacion = reader.GetBoolean(reader.GetOrdinal("enpublicacion")),
                Sinopsis = reader["sinopsis"].ToString()!,
                Calificacion = reader.GetDouble(reader.GetOrdinal("calificacion")),
                NumeroCapitulos = reader.GetInt32(reader.GetOrdinal("numerocapitulos")),
                Editorial = reader["editorial"].ToString()!,
                Estado = reader["estado"].ToString()!,
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fechacreacion")),
                FechaActualizacion = reader.IsDBNull(reader.GetOrdinal("fechaactualizacion")) ? null : reader.GetDateTime(reader.GetOrdinal("fechaactualizacion")),
                Precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                CantidadInventario = reader.GetInt32(reader.GetOrdinal("cantidadinventario")),
                TipoManga = reader["tipomanga"].ToString()!,
                InventarioActivo = reader.GetBoolean(reader.GetOrdinal("inventarioactivo"))
            });
        }
        return mangas;
    }

    public async Task<Manga?> GetByIdAsync(Guid id)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand("SELECT * FROM mangas WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapManga(reader);
        }
        return null;
    }

    public async Task<Manga> AddAsync(Manga manga)
    {
        Console.WriteLine($"Insertando manga: {System.Text.Json.JsonSerializer.Serialize(manga)}");
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand(@"INSERT INTO mangas (id, titulo, autor, genero_id, aniopublicacion, volumenes, enpublicacion, sinopsis, calificacion, numerocapitulos, editorial, estado, fechacreacion, fechaactualizacion, precio, cantidadinventario, tipomanga, inventarioactivo) VALUES (@id, @titulo, @autor, @genero_id, @aniopublicacion, @volumenes, @enpublicacion, @sinopsis, @calificacion, @numerocapitulos, @editorial, @estado, @fechacreacion, @fechaactualizacion, @precio, @cantidadinventario, @tipomanga, @inventarioactivo)", conn);
        cmd.Parameters.AddWithValue("@id", manga.Id);
        cmd.Parameters.AddWithValue("@titulo", manga.Titulo);
        cmd.Parameters.AddWithValue("@autor", manga.Autor);
        cmd.Parameters.AddWithValue("@genero_id", manga.GeneroId);
        cmd.Parameters.AddWithValue("@aniopublicacion", manga.AnioPublicacion);
        cmd.Parameters.AddWithValue("@volumenes", manga.Volumenes);
        cmd.Parameters.AddWithValue("@enpublicacion", manga.EnPublicacion);
        cmd.Parameters.AddWithValue("@sinopsis", manga.Sinopsis);
        cmd.Parameters.AddWithValue("@calificacion", manga.Calificacion);
        cmd.Parameters.AddWithValue("@numerocapitulos", manga.NumeroCapitulos);
        cmd.Parameters.AddWithValue("@editorial", manga.Editorial);
        cmd.Parameters.AddWithValue("@estado", manga.Estado);
        cmd.Parameters.AddWithValue("@fechacreacion", manga.FechaCreacion);
        cmd.Parameters.AddWithValue("@fechaactualizacion", (object?)manga.FechaActualizacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@precio", manga.Precio);
        cmd.Parameters.AddWithValue("@cantidadinventario", manga.CantidadInventario);
        cmd.Parameters.AddWithValue("@tipomanga", manga.TipoManga);
        cmd.Parameters.AddWithValue("@inventarioactivo", manga.InventarioActivo);
        await cmd.ExecuteNonQueryAsync();
        return manga;
    }

    public async Task<bool> UpdateAsync(Manga manga)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand(@"UPDATE mangas SET titulo=@titulo, autor=@autor, genero_id=@genero_id, aniopublicacion=@aniopublicacion, volumenes=@volumenes, enpublicacion=@enpublicacion, sinopsis=@sinopsis, calificacion=@calificacion, numerocapitulos=@numerocapitulos, editorial=@editorial, estado=@estado, fechacreacion=@fechacreacion, fechaactualizacion=@fechaactualizacion, precio=@precio, cantidadinventario=@cantidadinventario, tipomanga=@tipomanga, inventarioactivo=@inventarioactivo WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", manga.Id);
        cmd.Parameters.AddWithValue("@titulo", manga.Titulo);
        cmd.Parameters.AddWithValue("@autor", manga.Autor);
        cmd.Parameters.AddWithValue("@genero_id", manga.GeneroId);
        cmd.Parameters.AddWithValue("@aniopublicacion", manga.AnioPublicacion);
        cmd.Parameters.AddWithValue("@volumenes", manga.Volumenes);
        cmd.Parameters.AddWithValue("@enpublicacion", manga.EnPublicacion);
        cmd.Parameters.AddWithValue("@sinopsis", manga.Sinopsis);
        cmd.Parameters.AddWithValue("@calificacion", manga.Calificacion);
        cmd.Parameters.AddWithValue("@numerocapitulos", manga.NumeroCapitulos);
        cmd.Parameters.AddWithValue("@editorial", manga.Editorial);
        cmd.Parameters.AddWithValue("@estado", manga.Estado);
        cmd.Parameters.AddWithValue("@fechacreacion", manga.FechaCreacion);
        cmd.Parameters.AddWithValue("@fechaactualizacion", (object?)manga.FechaActualizacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@precio", manga.Precio);
        cmd.Parameters.AddWithValue("@cantidadinventario", manga.CantidadInventario);
        cmd.Parameters.AddWithValue("@tipomanga", manga.TipoManga);
        cmd.Parameters.AddWithValue("@inventarioactivo", manga.InventarioActivo);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand("DELETE FROM mangas WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<bool> ExistsByTitleAsync(string title)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand("SELECT 1 FROM mangas WHERE LOWER(titulo) = LOWER(@titulo) LIMIT 1", conn);
        cmd.Parameters.AddWithValue("@titulo", title);
        using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
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
        var added = new List<Manga>();
        foreach (var manga in mangas)
        {
            await AddAsync(manga);
            added.Add(manga);
        }
        return added;
    }

    private Manga MapManga(IDataRecord reader)
    {
        return new Manga
        {
            Id = reader.GetGuid(reader.GetOrdinal("id")),
            Titulo = reader["titulo"].ToString()!,
            Autor = reader["autor"].ToString()!,
            GeneroId = reader.GetInt32(reader.GetOrdinal("genero_id")),
            Genero = new Genero
            {
                Id = reader.GetInt32(reader.GetOrdinal("genero_id")),
                Nombre = reader["genero_nombre"].ToString()!
            },
            AnioPublicacion = reader.GetInt32(reader.GetOrdinal("aniopublicacion")),
            Volumenes = reader.GetInt32(reader.GetOrdinal("volumenes")),
            EnPublicacion = reader.GetBoolean(reader.GetOrdinal("enpublicacion")),
            Sinopsis = reader["sinopsis"].ToString()!,
            Calificacion = reader.GetDouble(reader.GetOrdinal("calificacion")),
            NumeroCapitulos = reader.GetInt32(reader.GetOrdinal("numerocapitulos")),
            Editorial = reader["editorial"].ToString()!,
            Estado = reader["estado"].ToString()!,
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fechacreacion")),
            FechaActualizacion = reader.IsDBNull(reader.GetOrdinal("fechaactualizacion")) ? null : reader.GetDateTime(reader.GetOrdinal("fechaactualizacion")),
            Precio = reader.GetDecimal(reader.GetOrdinal("precio")),
            CantidadInventario = reader.GetInt32(reader.GetOrdinal("cantidadinventario")),
            TipoManga = reader["tipomanga"].ToString()!,
            InventarioActivo = reader.GetBoolean(reader.GetOrdinal("inventarioactivo"))
        };
    }
} 