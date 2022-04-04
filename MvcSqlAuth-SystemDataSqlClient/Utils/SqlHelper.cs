using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace MvcSqlAuth_SystemDataSqlClient.Utils
{
    public class SqlHelper
    {
        public static string SqlAuthVmIdentity()
        {
            // https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/tutorial-windows-vm-access-sql
            // CREATE USER Dev02 FROM EXTERNAL PROVIDER    *** Dev02 is the VM Name
            // ALTER ROLE db_datareader ADD MEMBER Dev02

            string empLastName = "X";

            try
            {

                //
                // Get an access token for SQL.
                //
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=https://database.windows.net/");
                request.Headers["Metadata"] = "true";
                request.Method = "GET";
                string accessToken = null;

                try
                {
                    // Call managed identities for Azure resources endpoint.
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    // Pipe response Stream to a StreamReader and extract access token.
                    StreamReader streamResponse = new StreamReader(response.GetResponseStream());
                    string stringResponse = streamResponse.ReadToEnd();
                    JavaScriptSerializer j = new JavaScriptSerializer();
                    Dictionary<string, string> list = (Dictionary<string, string>)j.Deserialize(stringResponse, typeof(Dictionary<string, string>));
                    accessToken = list["access_token"];
                }
                catch (Exception e)
                {
                    string errorText = String.Format("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message : "Acquire token failed");
                }

                //
                // Open a connection to the server using the access token.
                //
                if (accessToken != null)
                {
                    string connectionString = "Data Source=deva-dhtest.database.windows.net; Initial Catalog=RMData;";
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.AccessToken = accessToken;
                    connection.Open();

                    String sql = "SELECT FirstName, LastName FROM Users";

                    using (SqlCommand command = new SqlCommand(sql, connection))
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

        public static string SqlAuthVmIdentityNew()
        {
            string empLastName = "X";
            try
            {
                //
                //  This will not work.  This will need Microsoft.Data.SqlClient (instead of System.Data.SqlClient)
                //
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