using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Domein.Domein;
using League.Domein.Exceptions;

namespace League.Domein
{
    public class Transfer
    {
        public int Id { get; private set; }
        public Speler Speler { get; private set; }
        public Team NieuwTeam { get; private set; }
        public Team OudTeam { get; private set; }
        public int Prijs { get; private set; }

        internal Transfer(Speler speler, Team nieuwTeam, Team oudTeam, int prijs)
        {
            ZetSpeler(speler);
            ZetNieuwTeam(nieuwTeam);
            ZetOudTeam(oudTeam);
            ZetPrijs(prijs);
        }
        // speler stopt
        public Transfer(Speler speler, Team oudTeam)
        {
            ZetSpeler(speler);
            ZetOudTeam(oudTeam);
            ZetPrijs(0);
        }
        //speler is nieuw
        public Transfer(Speler speler, Team nieuwTeam, int prijs)
        {
            ZetSpeler(speler);
            ZetNieuwTeam(nieuwTeam);
            ZetPrijs(prijs);
        }
        public void ZetId(int id)
        {
            if (id <= 0)
            {
                throw new TransferException("Zetid");
            }
            Id = id;
        }
        public void ZetPrijs(int prijs)
        {
            if (prijs < 0)
            {
                throw new TransferException("Zetprijs");
            }
            Prijs = prijs;
        }
        public void ZetSpeler(Speler speler)
        {
            if (speler is null)
            {
                throw new TransferException("Zetspeler");
            }
            Speler = speler;
        }
        public void VerwijderOudTeam()
        {
            if(NieuwTeam is null)
            {
                throw new TransferException("Verwijderoudteam"); // minstens 1 team
            }
            OudTeam = null;
        }
        public void ZetOudTeam(Team team)
        {
            if(team == null)
            {
                throw new TransferException("Zetoudteam");
            }
            if(team == NieuwTeam)
            {
                throw new TransferException("Zetoudteam");
            }
            OudTeam = team;
        }
        public void VerwijderNieuwTeam()
        {
            if(OudTeam is null)
            {
                throw new TransferException("Verwijdernieuwteam"); // minstens 1 team
            }
            NieuwTeam = null;
        }
        public void ZetNieuwTeam(Team team)
        {
            if (team == null)
            {
                throw new TransferException("Zetnieuwteam");
            }
            if (team == OudTeam)
            {
                throw new TransferException("Zetnieuwteam");
            }
            NieuwTeam = team;
        }
    }
}
