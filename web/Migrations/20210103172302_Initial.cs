using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prebivalisce",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mesto = table.Column<string>(nullable: true),
                    Naslov = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prebivalisce", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stik",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stik", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uporabnik",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(nullable: true),
                    Priimek = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefon = table.Column<string>(nullable: true),
                    PrebivalisceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uporabnik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uporabnik_Prebivalisce_PrebivalisceId",
                        column: x => x.PrebivalisceId,
                        principalTable: "Prebivalisce",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Odlok",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumZacetka = table.Column<DateTime>(nullable: false),
                    DatumKonca = table.Column<DateTime>(nullable: false),
                    UporabnikId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odlok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odlok_Uporabnik_UporabnikId",
                        column: x => x.UporabnikId,
                        principalTable: "Uporabnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Odlok_UporabnikId",
                table: "Odlok",
                column: "UporabnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Uporabnik_PrebivalisceId",
                table: "Uporabnik",
                column: "PrebivalisceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Odlok");

            migrationBuilder.DropTable(
                name: "Stik");

            migrationBuilder.DropTable(
                name: "Uporabnik");

            migrationBuilder.DropTable(
                name: "Prebivalisce");
        }
    }
}
