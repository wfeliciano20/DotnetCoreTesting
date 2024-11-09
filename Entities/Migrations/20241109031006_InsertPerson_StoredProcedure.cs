﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                string sp_InsertPerson = @"
                CREATE PROCEDURE [dbo].[InsertPerson]
                (@PersonID uniqueidentifier,@PersonName nvarchar(40), @Email nvarchar(40),
                @DateOfBirth datetime2(7), @Gender nvarchar(10), @CountryID uniqueidentifier,
                @Address nvarchar(200), @ReceiveNewsLetter bit)
                AS BEGIN
                    INSERT INTO [dbo].[People](PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetter)
                    VALUES (@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetter)
                END
            ";

            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                DROP PROCEDURE [dbo].[InsertPerson]
            ";

            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
