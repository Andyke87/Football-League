using League.Domein.Exceptions;
namespace League.Domein.Domein
{
    public class Speler
    {
        public int Id { get; private set; }
        public string Naam { get; private set; }
        public Team Team { get; private set; }
        public int? Rugnummer { get; private set; }
        public int? Lengte { get; private set; }
        public int? Gewicht { get; private set; }

        internal Speler(int id, string naam, int? lengte, int? gewicht) : this(naam, lengte, gewicht)
        {
            ZetId(id);
        }
        internal Speler(string naam, int? lengte, int? gewicht)
        {
            ZetNaam(naam);
            if (lengte != null)
            {
                ZetLengte(lengte.Value);
            }
            if (gewicht.HasValue)
            {
                ZetGewicht(gewicht.Value);
            }
        }

        public void ZetNaam(string naam)
        {
            if (string.IsNullOrEmpty(naam))
            {
                throw new SpelerException("Zetnaam");

            }
            Naam = naam.Trim();
        }
        public void ZetLengte(int lengte)
        {
            if (lengte < 150)
            {
                throw new SpelerException("Zetlengte");
            }
            Lengte = lengte;
        }
        public void ZetId(int id)
        {
            if (id < 0)
            {
                throw new SpelerException("Zetid");
            }
            Id = id;
        }
        public void ZetGewicht(int gewicht)
        {
            if (gewicht < 50)
            {
                throw new SpelerException("Zetgewicht");
            }
            Gewicht = gewicht;
        }
        public void ZetRugnummer(int rugnummer)
        {
            if (rugnummer < 0)
            {
                throw new SpelerException("Zetrugnummer");
            }
            Rugnummer = rugnummer;
        }

        internal void VerwijderTeam()
        {
            if (Team.HeeftSpeler(this))
                Team.VerwijderSpeler(this);
            Team = null;
        }

        internal void ZetTeam(Team team)
        {
            if (team == null)
            {
                throw new SpelerException("Zetteam");
            }
            if (team == Team)
            {
                throw new SpelerException("Zetteam");
            }
            if (Team == null)
            {
                if (Team.HeeftSpeler(this))
                {
                    Team.VerwijderSpeler(this);
                }
            }
            if (!team.HeeftSpeler(this)) team.VoegSpelerToe(this);
            Team = team;

        }
    }
}