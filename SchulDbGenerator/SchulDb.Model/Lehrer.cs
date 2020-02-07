﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchulDb.Model
{
    [Table("Lehrer")]
    public partial class Lehrer
    {
        public Lehrer()
        {
            Abteilungens = new HashSet<Abteilung>();
            InverseLChefNavigation = new HashSet<Lehrer>();
            Klassens = new HashSet<Klasse>();
            Pruefungens = new HashSet<Pruefung>();
            Stundens = new HashSet<Stunde>();
        }

        [Key]
        [Column("L_Nr")]
        [StringLength(8)]
        public string LNr { get; set; }
        [Required]
        [Column("L_Name")]
        [StringLength(255)]
        public string LName { get; set; }
        [Required]
        [Column("L_Vorname")]
        [StringLength(255)]
        public string LVorname { get; set; }
        [Column("L_Gebdat", TypeName = "datetime")]
        public DateTime? LGebdat { get; set; }
        [Column("L_Gehalt", TypeName = "money")]
        public decimal? LGehalt { get; set; }
        [Column("L_Chef")]
        [StringLength(8)]
        public string LChef { get; set; }
        [Column("L_Eintrittsjahr")]
        public int? LEintrittsjahr { get; set; }
        [Column("L_Sprechstunde")]
        [StringLength(8)]
        public string LSprechstunde { get; set; }

        [ForeignKey(nameof(LChef))]
        [InverseProperty(nameof(Lehrer.InverseLChefNavigation))]
        public virtual Lehrer LChefNavigation { get; set; }
        [InverseProperty(nameof(Abteilung.AbtLeiterNavigation))]
        public virtual ICollection<Abteilung> Abteilungens { get; set; }
        [InverseProperty(nameof(Lehrer.LChefNavigation))]
        public virtual ICollection<Lehrer> InverseLChefNavigation { get; set; }
        [InverseProperty(nameof(Klasse.KVorstandNavigation))]
        public virtual ICollection<Klasse> Klassens { get; set; }
        [InverseProperty(nameof(Pruefung.PPrueferNavigation))]
        public virtual ICollection<Pruefung> Pruefungens { get; set; }
        [InverseProperty(nameof(Stunde.StLehrerNavigation))]
        public virtual ICollection<Stunde> Stundens { get; set; }
    }
}