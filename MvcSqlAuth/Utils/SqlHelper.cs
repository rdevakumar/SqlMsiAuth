using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSqlAuth.Utils
{
    public class SqlHelper
    {

        public static string SqlAuthVmIdentity()
        {
            string empLastName = "X";
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
                                var empFirstName = reader.GetString(0);
                                empLastName = reader.GetString(1);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return "DB Query Failed";
            }
            return "DB Query Success: " + empLastName;
        }
    }
}