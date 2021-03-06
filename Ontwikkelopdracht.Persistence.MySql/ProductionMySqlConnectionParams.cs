﻿namespace Ontwikkelopdracht.Persistence.MySql
{
    /// <summary>
    ///     Connection parameters for the production environment.
    /// </summary>
    public class ProductionMySqlConnectionParams : IMySqlConnectionParams
    {
        public string Host { get; set; } = "db";
        public uint Port { get; set; } = 3306;
        public string Database { get; set; } = "se2";
        public string Username { get; set; } = "fontys";
        public string Password { get; set; } = "fontys";
    }
}