using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class DatabaseContext : DbContext
{
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Pokemon> Pokemons { get; set; }

    public string DbPath { get; }

    public DatabaseContext()
    {
        var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        DbPath = System.IO.Path.Join(projectDirectory, "sqlite.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.ToTable("Trainers");

            entity.HasKey(e => e.TrainerId);

            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Region).IsRequired();

            entity.HasMany(e => e.Pokemons)
                  .WithOne(p => p.Trainer)
                  .HasForeignKey(p => p.TrainerId);
        });

        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.ToTable("Pokemons");

            entity.HasKey(e => e.PokemonId);

            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Level).IsRequired();

            entity.HasOne(e => e.Trainer)
                  .WithMany(t => t.Pokemons)
                  .HasForeignKey(e => e.TrainerId);
        });

        base.OnModelCreating(modelBuilder);
    }
}

public class Trainer
{
    public int TrainerId { get; set; }
    public string Name { get; set; }
    public string Region { get; set; }

    public List<Pokemon> Pokemons { get; } = new();
}

public class Pokemon
{
    public int PokemonId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Level { get; set; }

    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; }
}