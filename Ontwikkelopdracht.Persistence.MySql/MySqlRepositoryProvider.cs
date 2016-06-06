using System;

namespace Ontwikkelopdracht.Persistence.MySql
{
    public class MySqlRepositoryProvider : IRepositoryProvider
    {
        public Type GetDatabaseType<T>() where T : new() => typeof(MySqlRepository<T>);
        public Type ConnectionParamsContract => typeof(IMySqlConnectionParams);
        public Type ConnectionParamsImpl => typeof(ProductionMySqlConnectionParams);
    }
}