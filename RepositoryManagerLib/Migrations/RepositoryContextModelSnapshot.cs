﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RepositoryManagerLib.Data;

#nullable disable

namespace RepositoryManagerLib.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    partial class RepositoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RepositoryManagerLib.Data.RepositoryItem", b =>
                {
                    b.Property<string>("ItemName")
                        .HasColumnType("text");

                    b.Property<string>("ItemContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer");

                    b.HasKey("ItemName");

                    b.ToTable("RepositoryItems");
                });
#pragma warning restore 612, 618
        }
    }
}
