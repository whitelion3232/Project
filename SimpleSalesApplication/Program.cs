using SimpleSalesApplication;
using System;
using System.Data.SqlClient;

namespace SimpleSalesApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=localhost;Initial Catalog=ProjectDB;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;Integrated Security=True;";

            SalesApplicationManager appManager = new SalesApplicationManager(connectionString);
            appManager.Run();
        }
    }
}