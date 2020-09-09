using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M120Projekt.Data
{
    public class Produkte
    {
        #region Datenbankschicht
        [Key]
        public Int64 PersonId { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public DateTime Lieferdatum { get; set; }
        [Required]
        public Boolean Verfuegbar { get; set; }
        [Required]
        public Int64 Preis { get; set; }
        [Required]
        public String Farbe { get; set; }
        #endregion
        #region Applikationsschicht
        public Produkte() { }
        [NotMapped]
        public String BerechnetesAttribut
        {
            get
            {
                return "Im Getter kann Code eingefügt werden für berechnete Attribute";
            }
        }
        public static List<Produkte> LesenAlle()
        {
            using (var db = new Context())
            {
                return (from record in db.Produkte select record).ToList();
            }
        }
        public static Produkte LesenID(Int64 klasseAId)
        {
            using (var db = new Context())
            {
                return (from record in db.Produkte where record.PersonId == klasseAId select record).FirstOrDefault();
            }
        }
        public static List<Produkte> LesenAttributGleich(String suchbegriff)
        {
            using (var db = new Context())
            {
                return (from record in db.Produkte where record.Name == suchbegriff select record).ToList();
            }
        }
        public static List<Produkte> LesenAttributWie(String suchbegriff)
        {
            using (var db = new Context())
            {
                return (from record in db.Produkte where record.Name.Contains(suchbegriff) select record).ToList();
            }
        }
        public Int64 Erstellen()
        {
            if (this.Name == null || this.Name == "") this.Name = "leer";
            if (this.Lieferdatum == null) this.Lieferdatum = DateTime.MinValue;
            using (var db = new Context())
            {
                db.Produkte.Add(this);
                db.SaveChanges();
                return this.PersonId;
            }
        }
        public Int64 Aktualisieren()
        {
            using (var db = new Context())
            {
                db.Entry(this).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return this.PersonId;
            }
        }
        public void Loeschen()
        {
            using (var db = new Context())
            {
                db.Entry(this).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public override string ToString()
        {
            return PersonId.ToString(); // Für bessere Coded UI Test Erkennung
        }
        #endregion
    }
}
