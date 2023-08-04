using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Domein.Domein;
using League.Domein.DTO;

namespace League.Domein
{
    public interface ITeamRepository
    {
        void SchrijfTeamInDB(Team t);
        

        Team SelecteerTeam(int stamnummer);

        void UpdateTeam(Team team);

        IReadOnlyList<TeamInfo> SelecteerTeams();
        bool BestaatTeam(int stamnummer);
    }
}
