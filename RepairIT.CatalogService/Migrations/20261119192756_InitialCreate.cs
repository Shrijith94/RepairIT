using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairIT.CatalogService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ordinateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Marque = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Processeur = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RamGo = table.Column<int>(type: "INTEGER", nullable: false),
                    StockageGo = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Etat = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Prix = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    QuantiteEnStock = table.Column<int>(type: "INTEGER", nullable: false),
                    DateAjout = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordinateurs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ordinateurs_Nom",
                table: "Ordinateurs",
                column: "Nom");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ordinateurs");
        }
    }
}
