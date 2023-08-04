using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Domein.Domein;
using League.Domein.Exceptions;

namespace League.Domein
{
    public class TransferRepositoryADO : ITransferRepository
    {
        private string connectionString;

        public TransferRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString); 
        }
        public Transfer SchrijfTransferInDB(Transfer transfer)
        {
            SqlConnection connection = GetConnection();

            string queryTransfer = "INSERT INTO dbo.Transfer(spelerid,prijs,oudteamid,nieuwteamid)"
                + "output INSERTED.ID VALUES(@spelerid,@prijs,@oudteamid,@nieuwteamid)";
            string querySpeler = "UPDATE spelers SET teamID=@teamid WHERE ID @id";

            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            using (SqlCommand commandSpeler = connection.CreateCommand())
            using (SqlCommand commandTransfer = connection.CreateCommand())
            {
                commandTransfer.Transaction = transaction;
                commandSpeler.Transaction = transaction;
                try
                {
                    commandTransfer.Parameters.Add(new SqlParameter("@spelerid", SqlDbType.Int));
                    commandTransfer.Parameters.Add(new SqlParameter("@prijs", SqlDbType.Int));
                    commandTransfer.Parameters.Add(new SqlParameter("@oudteamid", SqlDbType.Int));
                    commandTransfer.Parameters.Add(new SqlParameter("@nieuwteamid", SqlDbType.Int));
                    commandTransfer.CommandText = queryTransfer;
                    commandTransfer.Parameters["@spelerid"].Value = transfer.Speler.Id;
                    commandTransfer.Parameters["@prijs"].Value = transfer.Prijs;
                    if (transfer.OudTeam == null)
                        commandTransfer.Parameters["@oudteamid"].Value = DBNull.Value;
                    else
                        commandTransfer.Parameters["@oudteamid"].Value = transfer.OudTeam.Stamnummer;
                    if (transfer.NieuwTeam == null)
                        commandTransfer.Parameters["@nieuwteamid"].Value = DBNull.Value;
                    else
                        commandTransfer.Parameters["@nieuwteamid"].Value = transfer.NieuwTeam.Stamnummer;
                    int newId = (int)commandTransfer.ExecuteScalar();
                    transfer.ZetId(newId);

                    commandSpeler.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    commandSpeler.Parameters.Add(new SqlParameter("@teamid", SqlDbType.Int));
                    commandSpeler.CommandText = querySpeler;
                    commandSpeler.Parameters["@id"].Value = transfer.Speler.Id;
                    if (transfer.NieuwTeam == null)
                        commandSpeler.Parameters["@teamid"].Value = DBNull.Value;
                    else
                        commandSpeler.Parameters["@teamid"].Value = transfer.Speler.Team.Stamnummer;
                    commandSpeler.ExecuteNonQuery();
                    transaction.Commit();
                    return transfer;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new TransferRepositoryException("SchrijfTransferInDB", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
       /* public void UpdateTeam(Team team)
        {
            SqlConnection connection = GetConnection();
            string query = "UPDATE team SET naam=@naam, bijnaam=@bijnaam WHERE stamnummer = @stamnummer";
            using(SqlCommand command = connection.CreateCommand())
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
                    if(team.Bijnaam == null)
                        command.Parameters["@bijnaam"].Value = DBNull.Value;
                    else
                        command.Parameters["@bijnaam"].Value = team.Bijnaam;
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    throw new TeamRepositoryADOException("UpdateTeam", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }*/
    }
}
