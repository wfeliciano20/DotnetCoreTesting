﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class PeopleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("CountryId");

                    b.ToTable("countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("fb4fd053-27aa-4f82-b943-cb4808ce3918"),
                            CountryName = "USA"
                        },
                        new
                        {
                            CountryId = new Guid("da0c5257-0b7b-4e69-892d-3cdfa6d08564"),
                            CountryName = "Canada"
                        },
                        new
                        {
                            CountryId = new Guid("fee3ee7d-f715-4da2-8a0c-f70462cc26f7"),
                            CountryName = "UK"
                        },
                        new
                        {
                            CountryId = new Guid("06e68e72-5619-4b5b-8a00-fccda2c20fd9"),
                            CountryName = "Australia"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("CountryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("ReceiveNewsLetter")
                        .HasColumnType("bit");

                    b.HasKey("PersonID");

                    b.ToTable("people", (string)null);

                    b.HasData(
                        new
                        {
                            PersonID = new Guid("a70565d2-9c96-4271-8115-2328346c4f0a"),
                            Address = "17 Portage Terrace",
                            CountryID = new Guid("fb4fd053-27aa-4f82-b943-cb4808ce3918"),
                            DateOfBirth = new DateTime(1997, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "tstegers0@geocities.jp",
                            Gender = "Male",
                            PersonName = "Toddie",
                            ReceiveNewsLetter = true
                        },
                        new
                        {
                            PersonID = new Guid("d8f0b56e-0624-49ad-b825-133de7568287"),
                            Address = "88 Veith Street",
                            CountryID = new Guid("da0c5257-0b7b-4e69-892d-3cdfa6d08564"),
                            DateOfBirth = new DateTime(1990, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "eadamovicz1@yolasite.com",
                            Gender = "Male",
                            PersonName = "Eberhard",
                            ReceiveNewsLetter = false
                        },
                        new
                        {
                            PersonID = new Guid("ca482969-0d0d-4ce6-ba4e-2e3ddcb6bb99"),
                            Address = "13177 Rockefeller Avenue",
                            CountryID = new Guid("fee3ee7d-f715-4da2-8a0c-f70462cc26f7"),
                            DateOfBirth = new DateTime(1991, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "kspearett2@globo.com",
                            Gender = "Female",
                            PersonName = "Kassey",
                            ReceiveNewsLetter = true
                        },
                        new
                        {
                            PersonID = new Guid("6226f1c9-dda4-47eb-a7f5-8e7c8ac8d027"),
                            Address = "427 Armistice Plaza",
                            CountryID = new Guid("06e68e72-5619-4b5b-8a00-fccda2c20fd9"),
                            DateOfBirth = new DateTime(1993, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "kspringtorpe3@accuweather.com",
                            Gender = "Female",
                            PersonName = "Keriann",
                            ReceiveNewsLetter = true
                        },
                        new
                        {
                            PersonID = new Guid("d2d19459-7860-4dfb-93f2-3efb657ac6ad"),
                            Address = "2 Forest Park",
                            CountryID = new Guid("06e68e72-5619-4b5b-8a00-fccda2c20fd9"),
                            DateOfBirth = new DateTime(1990, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "fsedcole4@harvard.edu",
                            Gender = "Male",
                            PersonName = "Fred",
                            ReceiveNewsLetter = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
