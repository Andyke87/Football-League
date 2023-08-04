using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Domein.Domein;
using League.Domein.DTO;

namespace League.Domein.Interfaces
{
    public interface ISpelerRepository
    {
        Speler SchrijfSpelerInDB(Speler s);

        bool BestaatSpeler(Speler s);

        void UpdateSpeler(Speler s);
        Speler SelecteerSpeler(int id);

        IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam);
        bool BestaatSpeler(int id);
    }
}
