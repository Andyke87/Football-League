using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.Domein.DTO
{
    public class SpelerInfo
    {
        public SpelerInfo(int id, string naam, string team, int? rugnummer, int? lengte, int? gewicht)
        {
            Id = id;
            Naam = naam;
            Team = team;
            Rugnummer = rugnummer;
            Lengte = lengte;
            Gewicht = gewicht;
        }
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Team { get; set; }
        public int? Rugnummer { get; set; }
        public int? Lengte { get; set; }
        public int? Gewicht { get; set; }

        public override string ToString()
        {
            return $"{Id} {Naam} {Team} {Rugnummer} {Lengte} {Gewicht}";
        }
    }
}
