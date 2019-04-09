﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Pokemon.Data.Infrastructure
{
    public class PokemonContext : DbContext
    {
        public  DbSet<Core.Entities.Pokemon> Pokemon { get; set; }
        public  DbSet<Core.Entities.Evolution> Evolution { get; set; }
        public  DbSet<Core.Entities.Move> Move { get; set; }


        public PokemonContext(DbContextOptions<PokemonContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //EF Core 2.1+ Value conversion since EF can not store list of primitive types: https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions
            modelBuilder.Entity<Core.Entities.Pokemon>()
            .Property(e => e.Types)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            base.OnModelCreating(modelBuilder);
        }
    }
}
