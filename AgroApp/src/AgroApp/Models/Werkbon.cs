﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Werkbon
    {
        public User Gebruiker { set; get; }
        public DateTime Datum { set; get; }
        public Customer Klant { set; get; }
        public string Mankeuze { set; get; }
        public Machine[] Machines { set; get; }
        public Hulpstuk[] Hulpstukken { set; get; }
        public long VanTijd { set; get; }
        public long TotTijd { set; get; }
        public long TotaalTijd { set; get; }
        public long PauzeTijd { set; get; }
        public Gewicht[] Gewichten { set; get; }
        public Rijplaat IngaandeRijplaten { set; get; }
        public Rijplaat UitgaandeRijplaten { set; get; }
        public string VerbruikteMaterialen { set; get; }
        public string Opmerking { set; get; }
        public int? IdOpdrachtWerknemer { set; get; }

        public Werkbon()
        {
            
        }

        public Werkbon(User selectedGebruiker, DateTime datum, Customer klant, string mankeuze, int totaalTijd, Machine[] machines = null, Hulpstuk[] hulpstukken = null, int vanTijd = 0, int totTijd = 0, int pauzeTijd = 0, Gewicht[] gewichten = null, Rijplaat ingaandeRijplaten = null, Rijplaat uitgaandeRijplaten = null, string verbruikteMaterialen = "", string opmerking = "")
        {
            Gebruiker = selectedGebruiker;
            Datum = datum;
            Klant = klant;
            Mankeuze = mankeuze;
            Machines = machines;
            Hulpstukken = hulpstukken;
            VanTijd = vanTijd;
            TotTijd = totTijd;
            TotaalTijd = totaalTijd;
            PauzeTijd = pauzeTijd;
            Gewichten = gewichten;
            IngaandeRijplaten = ingaandeRijplaten;
            UitgaandeRijplaten = uitgaandeRijplaten;
            VerbruikteMaterialen = verbruikteMaterialen;
            Opmerking = opmerking;
        }
    }
}
