using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people", x => x.PersonID);
                });

            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("06e68e72-5619-4b5b-8a00-fccda2c20fd9"), "Australia" },
                    { new Guid("da0c5257-0b7b-4e69-892d-3cdfa6d08564"), "Canada" },
                    { new Guid("fb4fd053-27aa-4f82-b943-cb4808ce3918"), "USA" },
                    { new Guid("fee3ee7d-f715-4da2-8a0c-f70462cc26f7"), "UK" }
                });

            migrationBuilder.InsertData(
                table: "people",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetter" },
                values: new object[,]
                {
                    { new Guid("6226f1c9-dda4-47eb-a7f5-8e7c8ac8d027"), "427 Armistice Plaza", new Guid("06e68e72-5619-4b5b-8a00-fccda2c20fd9"), new DateTime(1993, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "kspringtorpe3@accuweather.com", "Female", "Keriann", true },
                    { new Guid("a70565d2-9c96-4271-8115-2328346c4f0a"), "17 Portage Terrace", new Guid("fb4fd053-27aa-4f82-b943-cb4808ce3918"), new DateTime(1997, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "tstegers0@geocities.jp", "Male", "Toddie", true },
                    { new Guid("ca482969-0d0d-4ce6-ba4e-2e3ddcb6bb99"), "13177 Rockefeller Avenue", new Guid("fee3ee7d-f715-4da2-8a0c-f70462cc26f7"), new DateTime(1991, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kspearett2@globo.com", "Female", "Kassey", true },
                    { new Guid("d2d19459-7860-4dfb-93f2-3efb657ac6ad"), "2 Forest Park", new Guid("06e68e72-5619-4b5b-8a00-fccda2c20fd9"), new DateTime(1990, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "fsedcole4@harvard.edu", "Male", "Fred", false },
                    { new Guid("d8f0b56e-0624-49ad-b825-133de7568287"), "88 Veith Street", new Guid("da0c5257-0b7b-4e69-892d-3cdfa6d08564"), new DateTime(1990, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "eadamovicz1@yolasite.com", "Male", "Eberhard", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "people");
        }
    }
}
