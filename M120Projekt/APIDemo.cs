using System;
using System.Diagnostics;

namespace M120Projekt
{
    static class APIDemo
    {
        #region KlasseA
        // Create
        public static void DemoACreate()
        {
            Debug.Print("--- DemoACreate ---");
            // KlasseA
            Data.Produkte klasseA1 = new Data.Produkte();
            klasseA1.Name = "BIT Flasche";
            klasseA1.Lieferdatum = DateTime.Today;
            klasseA1.Preis = 12;
            klasseA1.Verfuegbar = true;
            klasseA1.Farbe = "Blau";
            Int64 klasseA1Id = klasseA1.Erstellen();
            Debug.Print("Artikel erstellt mit Id:" + klasseA1Id);
        }
        public static void DemoACreateKurz()
        {
            Data.Produkte klasseA2 = new Data.Produkte { Name = "Ventilator", Verfuegbar = false, Lieferdatum = DateTime.Today, Preis = 16, Farbe = "Rot" };
            Int64 klasseA2Id = klasseA2.Erstellen();
            Debug.Print("Artikel erstellt mit Id:" + klasseA2Id);
        }

        // Read
        public static void DemoARead()
        {
            Debug.Print("--- DemoARead ---");
            // Demo liest alle
            foreach (Data.Produkte klasseA in Data.Produkte.LesenAlle())
            {
                Debug.Print("Artikel Id:" + klasseA.PersonId + " Name:" + klasseA.Name);
            }
        }
        // Update
        public static void DemoAUpdate()
        {
            Debug.Print("--- DemoAUpdate ---");
            // KlasseA ändert Attribute
            Data.Produkte klasseA1 = Data.Produkte.LesenID(1);
            klasseA1.Name = "Artikel 1 nach Update";
            klasseA1.Aktualisieren();
        }
        // Delete
        public static void DemoADelete()
        {
            Debug.Print("--- DemoADelete ---");
            Data.Produkte.LesenID(2).Loeschen();
            Debug.Print("Artikel mit Id 2 gelöscht");
        }
        #endregion
    }
}
