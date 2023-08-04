using League.Domein.Exceptions;

namespace League.Domein.Domein
{
    public class Team
    {
        public int Stamnummer { get; private set; }
        public string Naam { get; private set; }
        public string Bijnaam { get; private set; }

        private List<Speler> _speler = new List<Speler>();

        internal Team(int stamnummer, string naam)
        {
            ZetStamnummer(stamnummer);
            ZetNaam(naam);
        }
        public void ZetStamnummer(int stamnummer)
        {
            if (stamnummer <= 0)
            {
                throw new TeamException("Zetstamnummer");
            }
            Stamnummer = stamnummer;
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrEmpty(naam))
            {
                throw new TeamException("Zetnaam");
            }
            Naam = naam.Trim();
        }
        public void ZetBijnaam(string bijnaam)
        {
            if (string.IsNullOrEmpty(bijnaam))
            {
                throw new TeamException("Zetbijnaam");
            }
            Bijnaam = bijnaam.Trim();
        }
        internal void VerwijderSpeler(Speler speler)
        {
            if (speler == null)
            {
                throw new TeamException("Verwijderspeler");
            }
            if (!_speler.Contains(speler))
            {
                throw new TeamException("Verwijderspeler");
            }
            _speler.Remove(speler);
            if (speler.Team == this)
            {
                speler.VerwijderTeam();
            }
        }
        internal void VoegSpelerToe(Speler speler)
        {
            if (speler == null)
            {
                throw new TeamException("VoegspelerToe");
            }
            if (_speler.Contains(speler))
            {
                throw new TeamException("VoegspelerToe");
            }
            _speler.Add(speler);
            if (speler.Team != this)
            {
                speler.ZetTeam(this);
            }
        }
        public IReadOnlyList<Speler> Spelers()
        {
            return _speler.AsReadOnly();
        }

        internal bool HeeftSpeler(Speler speler)
        {
            return _speler.Contains(speler);
        }

        internal void VerwijderBijnaam()
        {
            Bijnaam = null;
        }
    }
}