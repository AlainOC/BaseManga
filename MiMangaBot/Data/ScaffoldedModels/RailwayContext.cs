using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiMangaBot.Data.ScaffoldedModels;

public partial class RailwayContext : DbContext
{
    public RailwayContext()
    {
    }

    public RailwayContext(DbContextOptions<RailwayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Manga> Mangas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ballast.proxy.rlwy.net;Port=47621;Database=railway;Username=postgres;Password=yDmdTTMaIbWnGRSiUdHCGJppuLnauGnz");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("generos_pkey");

            entity.ToTable("generos");

            entity.HasIndex(e => e.Nombre, "generos_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });

        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mangas_pkey");

            entity.ToTable("mangas");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Aniopublicacion).HasColumnName("aniopublicacion");
            entity.Property(e => e.Autor).HasColumnName("autor");
            entity.Property(e => e.Calificacion).HasColumnName("calificacion");
            entity.Property(e => e.Cantidadinventario).HasColumnName("cantidadinventario");
            entity.Property(e => e.Editorial).HasColumnName("editorial");
            entity.Property(e => e.Enpublicacion).HasColumnName("enpublicacion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fechaactualizacion).HasColumnName("fechaactualizacion");
            entity.Property(e => e.Fechacreacion).HasColumnName("fechacreacion");
            entity.Property(e => e.GeneroId).HasColumnName("genero_id");
            entity.Property(e => e.Inventarioactivo).HasColumnName("inventarioactivo");
            entity.Property(e => e.Numerocapitulos).HasColumnName("numerocapitulos");
            entity.Property(e => e.Precio)
                .HasPrecision(12, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Sinopsis).HasColumnName("sinopsis");
            entity.Property(e => e.Tipomanga).HasColumnName("tipomanga");
            entity.Property(e => e.Titulo).HasColumnName("titulo");
            entity.Property(e => e.Volumenes).HasColumnName("volumenes");

            entity.HasOne(d => d.Genero).WithMany(p => p.Mangas)
                .HasForeignKey(d => d.GeneroId)
                .HasConstraintName("mangas_genero_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
