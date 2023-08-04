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

namespace League.Domein
{
    public class TeamRepositoryADO : ITeamRepository
    {
        private string connectionString;

        public TeamRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        public bool BestaatTeam(int stamnummer)
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT COUNT(*) FROM dbo.Team WHERE Stamnummer = @Stamnummer";

            using(SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@Stamnummer", SqlDbType.Int));
                    command.CommandText = query;
                    command.Parameters["@Stamnummer"].Value = stamnummer;

                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                }
                catch (Exception ex)
                {
                    throw new TeamRepositoryException("BestaatTeam", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void SchrijfTeamInDB(Team team)
        {
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO dbo.Team (Stamnummer, Naam, Bijnaam) VALUES (@Stamnummer, @Naam, @Bijnaam)";

            using(SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@Stamnummer", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@Bijnaam", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@Naam"].Value = team.Naam;
                    command.Parameters["@Stamnummer"].Value = team.Stamnummer;                  
                    if(team.Bijnaam == null)
                    {
                        command.Parameters["@Bijnaam"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@Bijnaam"].Value = team.Bijnaam;
                    }

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new TeamRepositoryException("SchrijfTeamInDB", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public Team SelecteerTeam(int stamnummer)
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT t1.stamnummer,t1.naam as ploegnaam,t1.bijnaam,t2.*" +
                "From [dbo].[Team] t1 left join [dbo].[speler] t2 on t1.Stamnummer = t2.teamid" +
                "Where stamnummer=@stamnummer";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@stamnummer", SqlDbType.Int));
                command.CommandText = query;
                command.Parameters["@stamnummer"].Value = stamnummer;
                connection.Open();
                try
                {
                    Team team = null;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if(team == null)
                        {
                            string naam = (string)reader["ploegnaam"];
                            string bijnaam = null;
                            if(!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) bijnaam = (string)reader["bijnaam"];
                            team = new Team(stamnummer, naam);
                            if (bijnaam != null) team.ZetBijnaam(bijnaam);
                        }
                        if(!reader.IsDBNull(reader.GetOrdinal("spelerid")))
                        {
                            int? lengte = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int)reader["lengte"];
                            int? gewicht = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int)reader["gewicht"];
                            Speler speler = new Speler((int)reader["id"], (string)reader["naam"],lengte, gewicht);
                            speler.ZetTeam(team);
                            if(!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) 
                                speler.ZetRugnummer((int)reader["rugnummer"]);
                        }
                    }
                    reader.Close();
                    return team;
                }
                catch (Exception ex)
                {
                    throw new TeamRepositoryException("SelecteerTeam", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void UpdateTeam(Team team)
        {
            SqlConnection connection = GetConnection();
            string query = "UPDATE team SET naam=@naam, bijnaam=@bijnaam WHERE stamnummer = @stamnummer";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();

                try
                {
                    command.Parameters.Add(new SqlParameter("@stamnummer", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@bijnaam", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@stamnummer"].Value = team.Stamnummer;
                    command.Parameters["@naam"].Value = team.Naam;
                    if (team.Bijnaam == null)
                        command.Parameters["@bijnaam"].Value = DBNull.Value;
                    else
                        command.Parameters["@bijnaam"].Value = team.Bijnaam;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new TeamRepositoryADOException("UpdateTeam", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<TeamInfo> SelecteerTeams()
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT Stamnummer, Naam, Bijnaam FROM [dbo].[Team]";
            List<TeamInfo> teams = new List<TeamInfo>();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string bijnaam = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) bijnaam = (string)reader["bijnaam"];
                        teams.Add(new TeamInfo((int)reader["stamnummer"], (string)reader["naam"], bijnaam));
                    }
                    reader.Close();
                    return teams;
                }
                catch (Exception ex)
                {
                    throw new TeamRepositoryADOException("SelecteerTeams", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
