using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vjezba.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityUserToken<string>",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserToken<string>", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Stolovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Broj_Stolica = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stolovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobilni_broj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Stol = table.Column<int>(type: "int", nullable: true),
                    Id_Korisnika = table.Column<int>(type: "int", nullable: false),
                    Datum_Rezervacije = table.Column<DateOnly>(type: "date", nullable: false),
                    Vrijeme_Rezervacije = table.Column<TimeOnly>(type: "time", nullable: false),
                    Vrijeme_Trajanja_Rezervacije = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Stolovi_Id_Stol",
                        column: x => x.Id_Stol,
                        principalTable: "Stolovi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rezervacije_Users_Id_Korisnika",
                        column: x => x.Id_Korisnika,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Stolovi",
                columns: new[] { "Id", "Broj_Stolica" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_Id_Korisnika",
                table: "Rezervacije",
                column: "Id_Korisnika");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_Id_Stol",
                table: "Rezervacije",
                column: "Id_Stol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUserToken<string>");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Stolovi");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
