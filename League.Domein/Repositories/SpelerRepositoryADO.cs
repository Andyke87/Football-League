using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Domein.Domein;
using League.Domein.DTO;
using League.Domein.Exceptions;
using League.Domein.Interfaces;

namespace League.Domein.Repositories
{
    public class SpelerRepositoryADO : ISpelerRepository
    {
        private string connectionString;

        public SpelerRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public bool BestaatSpeler(Speler s)
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT COUNT(*) FROM dbo.Spelers WHERE Naam = @naam";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@naam"].Value = s.Naam;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                }
                catch (Exception ex)
                {
                    throw new SpelerRepositoryException("BestaatSpeler", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool BestaatSpeler(int id)
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT COUNT(*) FROM dbo.Spelers WHERE id=@id";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@id"].Value = id;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                }
                catch (Exception ex)
                {
                    throw new SpelerRepositoryADOException("BestaatSpeler", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Speler SchrijfSpelerInDB(Speler s)
        {
            SqlConnection conn = GetConnection();
            string query = "INSERT INTO dbo.Spelers (Naam, Lengte, Gewicht) " +
                "output INSERTED.ID VALUES(@naam,@lengte,@gewicht)";
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@lengte", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Int));
                    cmd.CommandText = query;
                    cmd.Parameters["@naam"].Value = s.Naam;
                    if (s.Lengte.HasValue)
                    {
                        cmd.Parameters["@lengte"].Value = s.Lengte.Value;
                    }
                    else
                    {
                        cmd.Parameters["@lengte"].Value = DBNull.Value;
                    }
                    if (s.Gewicht.HasValue)
                    {
                        cmd.Parameters["@gewicht"].Value = s.Gewicht.Value;
                    }
                    else
                    {
                        cmd.Parameters["@gewicht"].Value = DBNull.Value;
                    }
                    int id = (int)cmd.ExecuteScalar();
                    s.ZetId(id);
                    return s;
                }
            }
            catch (Exception ex)
            {
                throw new SpelerRepositoryException("SchrijfSpelerInDB", ex);
            }
            finally
            {
                conn.Close();
            }
        }

        public Speler SelecteerSpeler(int id)
        {
            SqlConnection connection = GetConnection();
            string query = "select ts.id soekerud,ts.naam spelernaam,ts.rugnummer spelerrugnummer," +
                "ts.lengte spelerlengte,ts.gewicht spelergewicht,ts.teamid spelerteamid,tt.*" +
                "from speler ts" +
                "left join (" +
                "Select t1.stamnummer,t1.naam as ploegnaam,t1.bijnaam,t2.*" +
                "FROM[dbo].[Team] t1 left join[dbo].[speler] t2 on t1.Stamnummer = t2.teamid " +
                ") tt on tt.stamnummer = ts.teamid" +
                "where ts.id = @id";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.CommandText = query;
                command.Parameters["@id"].Value = id;
                connection.Open();
                try
                {
                    Team team = null;
                    Speler speler = null;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if(speler == null)
                        {
                            int? lengte = null;
                            if(!reader.IsDBNull(reader.GetOrdinal("spelerlengte"))) lengte = (int?)reader["spelerlengte"];
                            int? gewicht = null;
                            if(!reader.IsDBNull(reader.GetOrdinal("spelergewicht"))) gewicht = (int?)reader["spelergewicht"];
                            speler = new Speler(id, (string)reader["spelernaam"],lengte,gewicht);
                            if(!reader.IsDBNull(reader.GetOrdinal("spelerrugnummer")))
                                speler.ZetRugnummer((int)reader["spelerrugnummer"]);
                            if(!reader.IsDBNull(reader.GetOrdinal("spelerteamid"))) return speler;
                           
                            
                        }
                        if (team == null)
                        {
                           string naam = (string)reader["ploegnaam"];
                           string bijnaam = null;
                           if(!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) bijnaam = (string)reader["bijnaam"];
                           team = new Team((int)reader["stamnummer"], naam);
                           if(bijnaam != null) team.ZetBijnaam(bijnaam);
                        }
                        if(!reader.IsDBNull(reader.GetOrdinal("id")))
                        {
                            int? lengte = null;
                            if(!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int?)reader["lengte"];
                            int? gewicht = null;
                            if(!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int?)reader["gewicht"];
                           int sid = (int)reader["id"];
                            Speler s = new Speler(sid, (string)reader["naam"], lengte, gewicht);
                            s.ZetTeam(team);
                            if(!reader.IsDBNull(reader.GetOrdinal("rugnummer")))
                                speler.ZetRugnummer((int)reader["rugnummer"]);
                            if (sid == id) speler = s;
                        }
                    }
                    reader.Close();
                    return speler;
                }
                catch (Exception ex)
                {
                    throw new SpelerRepositoryADOException("SelecteerSpeler", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam)
        {
            if((!id.HasValue) && (string.IsNullOrEmpty(naam) == true)) throw new SpelerRepositoryADOException("SelecteerSpelers - no valid input");
            string query = "SELECT id,t1.Naam,Rugnummer,Lengte,Gewicht," +
                "case when t2.Stamnummer is null then null" +
                "else concat(t2.Naam, ' (', t2.Bijnaam, ') - ',t2.Stamnummer)" +
                "end teamnaam" +
                "From Speler t1" +
                "left join team t2 on t1.teamid = t2.Stamnummer";
            if(id.HasValue) query += " WHERE t1.id=@id";
            else query += " WHERE t1.Naam=@naam";
            List<SpelerInfo> spelers = new List<SpelerInfo>();
            SqlConnection connection = GetConnection();
            using(SqlCommand command = connection.CreateCommand())
            {
                if (id.HasValue)
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.Parameters["@id"].Value = id.Value;
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@naam", SqlDbType.VarChar));
                    command.Parameters["@naam"].Value = naam;
                }
                command.CommandText = query;
                connection.Open();
                try
                {
                    IDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string teamnaam = null;
                        if(!reader.IsDBNull(reader.GetOrdinal("teamnaam"))) teamnaam = (string)reader["teamnaam"];
                        int? lengte = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int?)reader["lengte"];
                        int? gewicht = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int?)reader["gewicht"];
                        int? rugnummer = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) rugnummer = (int?)reader["rugnummer"];
                        SpelerInfo speler = new SpelerInfo((int)reader["id"], (string)reader["naam"], teamnaam,rugnummer,lengte,gewicht);
                        spelers.Add(speler);
                    }
                    reader.Close();
                    return spelers;
                }
                catch (Exception ex)
                {
                    throw new SpelerRepositoryADOException("SelecteerSpelers", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            
        }

        public void UpdateSpeler(Speler speler)
        {
            SqlConnection connection = GetConnection();
            string query = "UPDATE speler SET naam=@naam, rugnummer=@rugnummer,lengte=@lengte," +
                "gewicht=@gewicht WHERE id=@id";

            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@lengte", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@rugnummer", SqlDbType.Int));
                    command.CommandText = query;
                    command.Parameters["@id"].Value = speler.Id;
                    command.Parameters["@naam"].Value = speler.Naam;
                    if (speler.Lengte == null)
                        command.Parameters["@lengte"].Value = DBNull.Value;
                    else
                        command.Parameters["@lengte"].Value = speler.Lengte;
                    if (speler.Gewicht == null)
                        command.Parameters["@gewicht"].Value = DBNull.Value;
                    else
                        command.Parameters["@gewicht"].Value = speler.Gewicht;
                    if (speler.Rugnummer == null)
                        command.Parameters["@rugnummer"].Value = DBNull.Value;
                    else
                        command.Parameters["@rugnummer"].Value = speler.Rugnummer;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new SpelerRepositoryException("UpdateSpeler", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
