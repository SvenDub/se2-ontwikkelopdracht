using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inject;
using MySql.Data.MySqlClient;

namespace Ontwikkelopdracht.Persistence.MySql
{
    public class MySqlRepository<T, ID> : IRepository<T, ID> where T : new()
    {
        public IMySqlConnectionParams MySqlConnectionParams { set; protected get; } =
            Injector.Resolve<IMySqlConnectionParams>();

        private readonly EntityAttribute _entityAttribute;
        private readonly IdentityAttribute _identityAttribute;
        private readonly PropertyInfo _identityProperty;

        public MySqlRepository()
        {
            MemberInfo info = typeof(T);

            if (!info.GetCustomAttributes(true).Any(attr => attr is EntityAttribute))
            {
                throw new EntityException("The given type is not attributed with Entity");
            }

            _entityAttribute = info.GetCustomAttributes(true)
                .OfType<EntityAttribute>()
                .First();

            PropertyInfo[] properties = typeof(T).GetProperties();

            if (!properties.Any(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute))))
            {
                throw new EntityException("The given type has no property attributed with Identity");
            }

            _identityProperty = properties
                .First(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute)));
            _identityAttribute = _identityProperty.GetCustomAttributes(true)
                .OfType<IdentityAttribute>()
                .First();
        }

        public long Count()
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT COUNT(*) FROM {_entityAttribute.Table}";
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt64(result);
                }
            }
        }

        public void Delete(ID id)
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM {_entityAttribute.Table} WHERE {_identityAttribute.Column}=:Id";
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete<S>(List<S> entities) where S : T
        {
            entities.ForEach(s => Delete(s));
        }

        public void Delete(T entity)
        {
            Delete((ID) _identityProperty.GetGetMethod().Invoke(entity, new object[] {}));
        }

        public void DeleteAll()
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM {_entityAttribute.Table}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool Exists(ID id)
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT COUNT(*) FROM {_entityAttribute.Table} WHERE {_identityAttribute.Column}=:Id";
                    cmd.Parameters.AddWithValue("Id", id);
                    return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public List<T> FindAll()
        {
            //throw new NotImplementedException();
            return new List<T>();
        }

        public List<T> FindAll(List<ID> ids)
        {
            return ids.Select(FindOne).ToList();
        }

        public T FindOne(ID id)
        {
            //throw new NotImplementedException();
            return new T();
        }

        public S Save<S>(S entity) where S : T
        {
            //throw new NotImplementedException();
            return entity;
        }

        public List<S> Save<S>(List<S> entities) where S : T
        {
            return entities.Select(Save).ToList();
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

            MySqlConnection mySqlConnection = new MySqlConnection(mySqlConnectionStringBuilder.GetConnectionString(true));
            mySqlConnection.Open();
            return mySqlConnection;
        }
    }
}