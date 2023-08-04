using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Domein.Domein;
using League.Domein.DTO;
using League.Domein.Interfaces;

namespace League.Domein.Managers
{
    public class SpelerManager
    {
        private ISpelerRepository repo;

        public SpelerManager(ISpelerRepository repo)
        {
            this.repo = repo;
        }
        public Speler RegistreerSpeler(string naam, int? lengte, int? gewicht)
        {
            try
            {
                Speler s = new Speler(naam, lengte, gewicht);
                if (repo.BestaatSpeler(s))
                {
                    s = repo.SchrijfSpelerInDB(s);
                    return s;
                }
                else
                {
                    throw new SpelerManagerException("RegistreerSpeler - Speler bestaat al");
                }
            }
            catch (SpelerManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SpelerManagerException("RegistreerSpeler", ex);
            }

        }
        public void UpdateSpeler(SpelerInfo spelerInfo)
        {
            if(spelerInfo == null) throw new SpelerManagerException("UpdateSpeler - SpelerInfo is null");
            if(spelerInfo.Id == 0) throw new SpelerManagerException("UpdateSpeler - Id = 0");
            try
            {
                if (repo.BestaatSpeler(spelerInfo.Id))
                {
                    Speler speler = repo.SelecteerSpeler(spelerInfo.Id);
                    bool changed = false;
                    if(speler.Naam != spelerInfo.Naam)
                    {
                        speler.ZetNaam(spelerInfo.Naam);
                        changed = true;
                    }
                    if(((speler.Lengte.HasValue) && (speler.Lengte != spelerInfo.Lengte)) || ((spelerInfo.Lengte.HasValue) && (!speler.Lengte.HasValue)))
                    {
                        speler.ZetLengte((int)spelerInfo.Lengte);
                        changed = true;
                    }
                    if(((speler.Gewicht.HasValue) && (speler.Gewicht != spelerInfo.Gewicht)) || ((spelerInfo.Gewicht.HasValue) && (!speler.Gewicht.HasValue)))
                    {
                        speler.ZetGewicht((int)spelerInfo.Gewicht);
                        changed = true;
                    }
                    if(((speler.Rugnummer.HasValue) &&(speler.Rugnummer != spelerInfo.Rugnummer)) || ((spelerInfo.Rugnummer.HasValue) && (!speler.Rugnummer.HasValue)))
                    {
                        speler.ZetRugnummer((int)spelerInfo.Rugnummer);
                        changed = true;
                    }
                    if(!changed) throw new SpelerManagerException("UpdateSpeler - No changes");
                    repo.UpdateSpeler(speler);
                }
                else
                {
                    throw new SpelerManagerException("UpdateSpeler - Speler niet gevonden");
                }
            }
            catch (SpelerManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SpelerManagerException("UpdateSpeler", ex);
            }
        }
        public IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam)
        {
            if((id == null) && (string.IsNullOrWhiteSpace(naam))) throw new SpelerManagerException("SelecteerSpelers - no input");

            try
            {
                return repo.SelecteerSpelers(id, naam);
            }
            catch (SpelerManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SpelerManagerException("SelecteerSpelers", ex);
            }

        }
    }
}
