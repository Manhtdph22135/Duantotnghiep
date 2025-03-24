using System.Data;
using Microsoft.Data.SqlClient;

namespace API.Context
{
    public class DLBase
    {
        public DataTable GetData(string query)
        {
            string sqlConnectionString = @"Data Source=DEVMANH;Initial Catalog=Duantotnghiep;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        DataTable dataTable = new DataTable();
                        sqlDataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }
    }
}
