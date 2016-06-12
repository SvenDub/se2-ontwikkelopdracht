namespace Ontwikkelopdracht.Persistence.MySql
{
    /// <summary>
    ///     Interfaces that contains connection parameters for connecting to the MySQL server.
    /// </summary>
    public interface IMySqlConnectionParams
    {
        /// <summary>
        ///     Hostname or IP.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        ///     Port. (Usually 3306)
        /// </summary>
        uint Port { get; set; }

        /// <summary>
        ///     Database name.
        /// </summary>
        string Database { get; set; }

        /// <summary>
        ///     Username.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        ///     Password.
        /// </summary>
        string Password { get; set; }
    }
}