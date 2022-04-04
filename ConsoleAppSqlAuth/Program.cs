using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSqlAuth
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string ConnectionString1 = @"Server=deva-dhtest.database.windows.net; Authentication=Active Directory Managed Identity; Database=RMData;";

                using (SqlConnection conn = new SqlConnection(ConnectionString1))
                {
                    conn.Open();
                    String sql = "SELECT FirstName, LastName FROM Users";

                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());

            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
