using System;
using System.Collections.Generic;
using ImageInDbApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageInDbApi.Context;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DbImage> DbImages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=postgres;Username=postgres;Password=postgres;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("PM02", "pgcrypto")
            .HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<DbImage>(entity =>
        {
            entity.HasKey(e => e.DbImageId).HasName("db_images_pk");

            entity.ToTable("db_images", "imageInDB");

            entity.Property(e => e.DbImageId).HasColumnName("db_image_id");
            entity.Property(e => e.DbImageBytea).HasColumnName("db_image_bytea");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
