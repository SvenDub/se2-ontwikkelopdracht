using System;
using System.IO;
using System.Reflection;
using Inject;
using MySql.Data.MySqlClient;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Persistence.MySql
{
    public class MySqlSetup : IRepositorySetup
    {
        public IMySqlConnectionParams MySqlConnectionParams { set; protected get; } =
            Injector.Resolve<IMySqlConnectionParams>();

        public bool Setup()
        {
            Log.I("DB", "Initializing database.");

            try
            {
                using (MySqlConnection connection = CreateConnection())
                {
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = GetResourceFileContentAsString("CREATE.sql");
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception e)
            {
                Log.E("DB", "Could not initialize database.");
                Log.E("DB", e.ToString());
                return false;
            }

            Log.I("DB", "Database initialized.");
            return true;
        }

        private MySqlConnection CreateConnection()
        {
            MySqlConnectionStringBuilder mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Server = MySqlConnectionParams.Host,
                Port = MySqlConnectionParams.Port,
                UserID = MySqlConnectionParams.Username,
                Password = MySqlConnectionParams.Password,
                Database = MySqlConnectionParams.Database
            };

            try
            {
                MySqlConnection mySqlConnection =
                    new MySqlConnection(mySqlConnectionStringBuilder.GetConnectionString(true));
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (System.Exception e)
            {
                throw new ConnectException(e);
            }
        }

        public static string GetResourceFileContentAsString(string fileName)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "Ontwikkelopdracht.Persistence.MySql." + fileName;

                string resource = null;
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        resource = reader.ReadToEnd();
                    }
                }
                return resource;
            }
    }
}