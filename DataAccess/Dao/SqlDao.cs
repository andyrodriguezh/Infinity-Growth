using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Dao
{
    public class SqlDao
    {
        private string _connectionString = "Server=tcp:infinitygrowth.database.windows.net,1433;Initial Catalog=InfinityGrowth;Persist Security Info=False;User ID=AdminIG;Password=FuerzaLeona123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";


        public static SqlDao? intance;

        public static SqlDao GetInstance()
        {
            if (intance == null)
            {
                intance = new SqlDao();
            }
            return intance;
        }

        // ExecuteStoreProcedure CREATE, UPDATE, DELETE
        public void ExecuteStoreProcedure(SqlOperation pOperation)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                var command = connection.CreateCommand();

                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = pOperation.ProcedureName;

                foreach (var item in pOperation.Parameters)
                {
                    command.Parameters.Add(item);
                }

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ExecuteStoreProcedure SELECT
        public List<Dictionary<string, object>> ExecuteStoreProcedureWithQuery(SqlOperation pOperation)
        {
            try
            {
                var listResult = new List<Dictionary<string, object>>();

                var connection = new SqlConnection(_connectionString);
                var command = connection.CreateCommand();

                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = pOperation.ProcedureName;

                foreach (var item in pOperation.Parameters)
                {
                    command.Parameters.Add(item);
                }

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetName(i), reader.GetValue(i));
                        }
                        listResult.Add(row);
                    }
                }

                connection.Close();
                return listResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
        }


    }
}
