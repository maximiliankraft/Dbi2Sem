﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SchulDb.Model
{
    public partial class SchuleContext : DbContext
    {
        public SchuleContext()
        {
        }

        public SchuleContext(DbContextOptions<SchuleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Abteilung> Abteilungens { get; set; }
        public virtual DbSet<Gegenstand> Gegenstaendes { get; set; }
        public virtual DbSet<Geschlecht> Geschlechters { get; set; }
        public virtual DbSet<Klasse> Klassens { get; set; }
        public virtual DbSet<Lehrer> Lehrers { get; set; }
        public virtual DbSet<Pruefung> Pruefungens { get; set; }
        public virtual DbSet<Raum> Raeumes { get; set; }
        public virtual DbSet<Religion> Religionens { get; set; }
        public virtual DbSet<Schueler> Schuelers { get; set; }
        public virtual DbSet<Schuljahr> Schuljahres { get; set; }
        public virtual DbSet<Staat> Staatens { get; set; }
        public virtual DbSet<Stunde> Stundens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pruefung>(entity =>
            {
                entity.HasKey(e => new { e.PDatumZeit, e.PPruefer, e.PGegenstand });
            });

            modelBuilder.Entity<Stunde>(entity =>
            {
                entity.HasKey(e => new { e.StStunde, e.StTag, e.StLehrer });
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}