﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pokemon.Api.Infrastructure.Data;

namespace Pokemon.Api.Infrastructure.Migrations
{
    [DbContext(typeof(PokemonContext))]
    [Migration("20190508191924_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("Pokemon.Api.Core.Entities.Evolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Event");

                    b.Property<int>("Pokemon");

                    b.Property<int>("PokemonId");

                    b.HasKey("Id");

                    b.HasIndex("PokemonId");

                    b.ToTable("Evolution");
                });

            modelBuilder.Entity("Pokemon.Api.Core.Entities.Move", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Accuracy");

                    b.Property<string>("Attack");

                    b.Property<string>("Category");

                    b.Property<string>("Description");

                    b.Property<string>("EffectPercent");

                    b.Property<string>("Level");

                    b.Property<string>("Name");

                    b.Property<string>("PP");

                    b.Property<int>("PokemonId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("PokemonId");

                    b.ToTable("Move");
                });

            modelBuilder.Entity("Pokemon.Api.Core.Entities.Pokemon", b =>
                {
                    b.Property<int>("PokemonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageUrl");

                    b.Property<int>("Index");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Types");

                    b.HasKey("PokemonId");

                    b.ToTable("Pokemon");
                });

            modelBuilder.Entity("Pokemon.Api.Core.Entities.Evolution", b =>
                {
                    b.HasOne("Pokemon.Api.Core.Entities.Pokemon")
                        .WithMany("Evolutions")
                        .HasForeignKey("PokemonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pokemon.Api.Core.Entities.Move", b =>
                {
                    b.HasOne("Pokemon.Api.Core.Entities.Pokemon")
                        .WithMany("Moves")
                        .HasForeignKey("PokemonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
