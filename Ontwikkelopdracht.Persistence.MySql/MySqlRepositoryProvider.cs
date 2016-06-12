using System;

namespace Ontwikkelopdracht.Persistence.MySql
{
    /// <see cref="MySqlRepository{T}"/>
    public class MySqlRepositoryProvider : IRepositoryProvider
    {
        public Type GetDatabaseType<T>() where T : new() => typeof(MySqlRepository<T>);
        public Type ConnectionParamsContract => typeof(IMySqlConnectionParams);
        public Type ConnectionParamsImpl => typeof(ProductionMySqlConnectionParams);
        public Type Setup => typeof(MySqlSetup);
    }
}