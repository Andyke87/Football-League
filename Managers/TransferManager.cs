using League.Domein.Domein;
using League.Domein.DTO;
using League.Domein.Interfaces;
using League.Domein.Managers;
using League.Domein.Repositories;
using League.Domein.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.Domein
{
    public class TransferManager
    {
        private ITeamRepository teamRepo;
        private ISpelerRepository spelerRepo;
        private ITransferRepository repo;

        public TransferManager(ITransferRepository repo, ITeamRepository teamRepo, ISpelerRepository spelerRepo)
        {
            this.repo = repo;
            this.teamRepo = teamRepo;
            this.spelerRepo = spelerRepo;
        }
        public Transfer RegistreerTransfer(SpelerInfo spelerInfo, TeamInfo nieuwTeamInfo,int prijs)
        {
            if(spelerInfo == null)throw new TransferManagerException("RegistreerTransfer - speler is null");
            if(spelerInfo.Id == 0) throw new TransferManagerException("RegistreerTransfer - speler id is 0");
            Transfer transfer = null;
            try
            {
                if(nieuwTeamInfo == null)
                {
                    if(spelerInfo.Team == null) throw new TransferManagerException("RegistreerTransfer - team is null");
                    Speler speler = spelerRepo.SelecteerSpeler(spelerInfo.Id);
                    transfer = new Transfer(speler, speler.Team);
                    speler.VerwijderTeam();
                }
                else if(spelerInfo.Team == null)
                {
                    Speler speler = spelerRepo.SelecteerSpeler(spelerInfo.Id);
                    Team team = teamRepo.SelecteerTeam(nieuwTeamInfo.Stamnummer);
                    speler.ZetTeam(team);
                    transfer = new Transfer(speler, team, prijs);
                }
                else
                {
                    Speler speler = spelerRepo.SelecteerSpeler(spelerInfo.Id);
                    Team team = teamRepo.SelecteerTeam(nieuwTeamInfo.Stamnummer);
                    transfer = new Transfer(speler, team, speler.Team, prijs);
                    speler.ZetTeam(team);
                }
                return repo.SchrijfTransferInDB(transfer);
            }
            catch(TransferManagerException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new TransferManagerException("RegistreerTransfer", ex);
            }
        }
    }
}
