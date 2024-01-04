
using Microsoft.Data.SqlClient;

namespace AdonetEXERSICEBD
{
    public class Program
    {
        const string connectionString = @"Server=DESKTOP-88S8DPK\SQLEXPRESS01;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=true";

     

        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

        }


    }
}
