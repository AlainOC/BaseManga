using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MiMangaBot.Migrations
{
    /// <inheritdoc />
    public partial class Baseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genero", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mangas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Autor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AnioPublicacion = table.Column<int>(type: "integer", nullable: false),
                    Volumenes = table.Column<int>(type: "integer", nullable: false),
                    EnPublicacion = table.Column<bool>(type: "boolean", nullable: false),
                    Sinopsis = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Calificacion = table.Column<double>(type: "double precision", nullable: false),
                    NumeroCapitulos = table.Column<int>(type: "integer", nullable: false),
                    Editorial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Precio = table.Column<decimal>(type: "numeric", nullable: false),
                    CantidadInventario = table.Column<int>(type: "integer", nullable: false),
                    TipoManga = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InventarioActivo = table.Column<bool>(type: "boolean", nullable: false),
                    GeneroId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mangas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mangas_Genero_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Genero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_GeneroId",
                table: "Mangas",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_Titulo",
                table: "Mangas",
                column: "Titulo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mangas");

            migrationBuilder.DropTable(
                name: "Genero");
        }
    }
}
