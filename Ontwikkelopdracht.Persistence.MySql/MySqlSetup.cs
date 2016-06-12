using System.IO;
using System.Reflection;
using Inject;
using MySql.Data.MySqlClient;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Persistence.MySql
{
    /// <summary>
    ///     Provides intilalization of the database by creating the tables and inserting initial data.
    /// </summary>
    public class MySqlSetup : IRepositorySetup
    {
        /// <summary>
        ///     Connection parameters to use.
        /// </summary>
        public IMySqlConnectionParams MySqlConnectionParams { set; protected get; } =
            Injector.Resolve<IMySqlConnectionParams>();

        /// <summary>
        ///     Execute the <c>CREATE.sql</c> script.
        /// </summary>
        public bool Setup()
        {
            Log.I("DB", "Initializing database.");

            try
            {
                using (MySqlConnection connection = CreateConnection())
                {
                    // Wrap in transaction. DDL auto commits, but rolls back inserts
                    MySqlTransaction transaction = connection.BeginTransaction();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        try
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = GetResourceFileContentAsString("CREATE.sql");
                            cmd.ExecuteNonQuery();

                            transaction.Commit();
                        }
                        catch (System.Exception e)
                        {
                            // Roll back when the script fails
                            Log.E("DB", "Could not initialize database.");
                            Log.E("DB", e.ToString());
                            Log.I("DB", "Rolling back.");

                            try
                            {
                                transaction?.Rollback();
                                Log.I("DB", "Rollback success.");
                            }
                            catch (System.Exception e1)
                            {
                                Log.E("DB", "Could not roll back.");
                                Log.E("DB", e1.ToString());
                            }

                            return false;
                        }
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

        /// <summary>
        ///     Open a new connection to the database.
        /// </summary>
        /// <returns>The open connection.</returns>
        /// <exception cref="ConnectException">When the connection could not be established.</exception>
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

        /// <summary>
        ///     Get the contents of an <c>Embedded Resource</c>.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The contents of the file.</returns>
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