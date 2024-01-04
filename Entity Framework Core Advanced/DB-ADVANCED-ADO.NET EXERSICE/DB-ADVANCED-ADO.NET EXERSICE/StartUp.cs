
using DB_ADVANCED_ADO.NET_EXERSICE;
using Microsoft.Data.SqlClient;


public class StartUp
{

    static void Main(string[] args)
    {

        using SqlConnection connection =
           new SqlConnection(Config.ConnectionString);
        connection.Open();
        Console.WriteLine("Connected to db");
    }



}
