﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using System.Linq;

namespace Lieferservice
{

    public class LieferserviceContext : DbContext
    {
        public DbSet<Bestellung> Bestellungen { get; set; }
        public DbSet<Kategorie> Kategorien { get; set; }
        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<Liefergebiet> Liefergebiete { get; set; }
        public DbSet<Produkt> Produkte { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //    @"Server=(localdb)\mssqllocaldb;Database=Lieferservice;Integrated Security=True");
            optionsBuilder.UseSqlite("Data Source=Lieferservice.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Liefergebiet>().HasKey(l => new { l.Plz, l.Ort });
            modelBuilder.Entity<ProduktBestellung>().HasKey("BestellungId", "ProduktId");
            modelBuilder.Entity<Kunde>().HasIndex(k => k.Email).IsUnique();
        }

        internal void Seed()
        {
            Randomizer.Seed = new Random(8675309);
            Faker f = new Faker();

            var kategorien = new List<Kategorie>
            {
                new Kategorie {Name = "Pizza"},
                new Kategorie {Name = "Pasti"},
                new Kategorie {Name = "Pesce"},
            };
            Kategorien.AddRange(kategorien); SaveChanges();

            var produkte = new List<Produkt>
            {
                new Produkt {Name = "Marinara", Kategorie = kategorien[0], Preis = 5.00M},
                new Produkt {Name = "Margherita", Kategorie = kategorien[0], Preis = 6.00M},
                new Produkt {Name = "Cipolla", Kategorie = kategorien[0], Preis = 7.00M},
                new Produkt {Name = "Alla Napoletana", Kategorie = kategorien[1], Preis = 6.50M},
                new Produkt {Name = "All Arabiata", Kategorie = kategorien[1], Preis = 7.50M},
                new Produkt {Name = "Alla Bolognese", Kategorie = kategorien[1], Preis = 7.50M},
                new Produkt {Name = "Schollenfilet gebacken", Kategorie = kategorien[2], Preis = 9.50M},
                new Produkt {Name = "Natur gebratenes Schollenfilet", Kategorie = kategorien[2], Preis = 9.50M},
                new Produkt {Name = "Miesmuscheln", Kategorie = kategorien[2], Preis = 11.50M},
            };
            Produkte.AddRange(produkte); SaveChanges();

            var liefergebiete = new List<Liefergebiet>
            {
                new Liefergebiet {Plz = 1040, Ort = "Wien", Lieferzuschlag = 5M  },
                new Liefergebiet {Plz = 1050, Ort = "Wien", Lieferzuschlag = 5M  },
                new Liefergebiet {Plz = 1060, Ort = "Wien", Lieferzuschlag = 5M  },
                new Liefergebiet {Plz = 1160, Ort = "Wien", Lieferzuschlag = 8M  },
                new Liefergebiet {Plz = 1170, Ort = "Wien", Lieferzuschlag = 8M  },
                new Liefergebiet {Plz = 1180, Ort = "Wien", Lieferzuschlag = 8M  }
            };
            Liefergebiete.AddRange(liefergebiete); SaveChanges();

            var kunden = new List<Kunde>
            {
                new Kunde {Vorname = "Lukas", Zuname = "Müller", Email = "lukas@mail.at"},
                new Kunde {Vorname = "Konstantin", Zuname = "Schmidt", Email = "konstantin@mail.at"},
                new Kunde {Vorname = "Ben", Zuname = "Schneider", Email = "ben@mail.at"},
                new Kunde {Vorname = "Jonas", Zuname = "Fischer", Email = "jonas@mail.at"},
                new Kunde {Vorname = "Elias", Zuname = "Weber", Email = "elias@mail.at"},
                new Kunde {Vorname = "Niklas", Zuname = "Meyer", Email = "niklas@mail.at"},
                new Kunde {Vorname = "David", Zuname = "Wagner", Email = "david@mail.at"},
                new Kunde {Vorname = "Oskar", Zuname = "Becker", Email = "oskar@mail.at"},
                new Kunde {Vorname = "Philipp", Zuname = "Schulz", Email = "philipp@mail.at"}
            };

            foreach (var k in f.Random.ListItems(kunden, (int)(0.8 * kunden.Count)))
            {
                var bestellungen = new Faker<Bestellung>().Rules((f, b) =>
                {
                    b.Adresse = f.Address.StreetAddress();
                    b.Liefergebiet = f.Random.ListItem(liefergebiete);
                    b.Kunde = f.Random.ListItem(kunden);
                    b.Bestellzeit = new DateTime(2020, 5, 1, 0, 0, 0).AddSeconds(f.Random.Int(0, 5 * 86400));
                }).Generate(f.Random.Int(1,3));
                foreach (var b in bestellungen)
                {
                    var produktBestellungen = new Faker<ProduktBestellung>().Rules((f, p) =>
                    {
                        p.Menge = f.Random.Int(1, 3);
                        p.Produkt = f.Random.ListItem(produkte);
                        p.Bestellung = b;
                    }).Generate(f.Random.Int(1, 5));
                    b.ProduktBestellungen = produktBestellungen
                        .GroupBy(pb => new { pb.Bestellung, pb.Produkt })
                        .Select(g => g.FirstOrDefault())
                        .ToList();
                }
                k.Bestellungen = bestellungen;
            }
            Kunden.AddRange(kunden); SaveChanges();
        }
    }

}
