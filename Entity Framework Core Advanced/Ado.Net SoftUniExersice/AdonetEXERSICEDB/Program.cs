using AdonetEXERSICEDB;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace AdonetEXERSICEBD
{
    public class Program
    {
        const string connectionString = @"Server=DESKTOP-88S8DPK\SQLEXPRESS01;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=true";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            using SqlCommand getvilliansCommand = new SqlCommand(sqlQuerry.GetVilliansWithMinions, sqlConnection);

            using SqlDataReader sqlDataReader = getvilliansCommand.ExecuteReader();


            while (sqlDataReader.Read())
            {
                Console.WriteLine($"{sqlDataReader["Name"]} - {sqlDataReader["MinionsCount"]}");
            }

        }



    }




}