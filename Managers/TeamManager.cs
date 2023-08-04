using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Domein.Domein;
using League.Domein.DTO;
using League.Domein.Exceptions;

namespace League.Domein.Managers
{
    public class TeamManager
    {
        private ITeamRepository repo;

        public TeamManager(ITeamRepository repo)
        {
            this.repo = repo;
        }
        public void RegistreerTeam(int stamnummer, string naam, string bijnaam)
        {
            try
            {
                Team t = new Team(stamnummer, naam);
                if (!string.IsNullOrEmpty(bijnaam)) t.ZetBijnaam(bijnaam);
                if (repo.BestaatTeam(stamnummer))
                {
                    repo.SchrijfTeamInDB(t);
                }
                else
                {
                    throw new TeamManagerException("RegistreerTeam - Team bestaat al");
                }
            }
            catch (TeamManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TeamManagerException("RegistreerTeam", ex);
            }
        }
        public Team SelecteerTeam(int stamnummer)
        {
            try
            {
                Team team = repo.SelecteerTeam(stamnummer);
                if (team == null) throw new TeamManagerException("SelecteerTeam - Team bestaat niet");
                return team;
            }
            catch (TeamManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TeamManagerException("SelecteerTeam", ex);
            }
        }
        public void UpdateTeam(TeamInfo teamInfo)
        {
            if (teamInfo == null) throw new TeamManagerException("UpdateTeam - Team is null");
            if(string.IsNullOrEmpty(teamInfo.Naam)) throw new TeamManagerException("UpdateTeam - teamnaam is null");

            try
            {
                if (repo.BestaatTeam(teamInfo.Stamnummer))
                {
                    Team team = repo.SelecteerTeam(teamInfo.Stamnummer);
                    team.ZetNaam(teamInfo.Naam);
                    if(!string.IsNullOrEmpty(teamInfo.Bijnaam)) team.ZetBijnaam(teamInfo.Bijnaam);
                    else team.VerwijderBijnaam();
                    repo.UpdateTeam(team);
                }
                else
                {
                    throw new TeamManagerException("UpdateTeam - Team niet gevonden");
                }
            }
            catch (TeamManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TeamManagerException("UpdateTeam", ex);
            }
        }
        public IReadOnlyList<TeamInfo> SelecteerTeams()
        {
            try
            {
                return repo.SelecteerTeams();
            }
            catch (Exception ex)
            {
                throw new TeamManagerException("SelecteerTeams", ex);
            }
        }

    }
}
