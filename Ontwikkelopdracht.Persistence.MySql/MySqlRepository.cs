using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Inject;
using MySql.Data.MySqlClient;
using Ontwikkelopdracht.Models;

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

            Log.D("DB", _entityAttribute.Table + " [ " + string.Join(", ", DataMembers.Values) + " ]");
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
                    cmd.CommandText =
                        $"SELECT COUNT(*) FROM {_entityAttribute.Table} WHERE {_identityAttribute.Column}=:Id";
                    cmd.Parameters.AddWithValue("Id", id);
                    return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public List<T> FindAll()
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT `{string.Join("`, `", DataMembers.Values)}` " +
                                      $"FROM {_entityAttribute.Table}";

                    return CreateListFromReader(cmd.ExecuteReader()).ToList();
                }
            }
        }

        public List<T> FindAll(List<ID> ids)
        {
            return ids.Select(FindOne).ToList();
        }

        public T FindOne(ID id)
        {
            throw new NotImplementedException();
        }

        public S Save<S>(S entity) where S : T
        {
            throw new NotImplementedException();
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

        private Dictionary<PropertyInfo, string> DataMembers => typeof(T)
            .GetProperties()
            .Where(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute), true))
            .Select(
                propertyInfo =>
                    new KeyValuePair<PropertyInfo, string>(propertyInfo,
                        propertyInfo.GetCustomAttribute<IdentityAttribute>(true).Column))
            .Concat(
                typeof(T)
                    .GetProperties()
                    .Where(propertyInfo => propertyInfo.IsDefined(typeof(DataMemberAttribute), true))
                    .Select(
                        propertyInfo =>
                            new KeyValuePair<PropertyInfo, string>(propertyInfo,
                                propertyInfo.GetCustomAttribute<DataMemberAttribute>(true).Column))
            ).ToDictionary(pair => pair.Key, pair => pair.Value);

        private IEnumerable<T> CreateListFromReader(MySqlDataReader reader)
        {
            using (reader)
            {
                while (reader.Read())
                {
                    T entity = new T();
                    foreach (var keyValuePair in DataMembers)
                    {
                        Log.D("DB", $"{keyValuePair.Value} = {reader[keyValuePair.Value]}");
                        if (keyValuePair.Key.IsDefined(typeof(IdentityAttribute)))
                        {
                            keyValuePair.Key.SetValue(entity, reader[keyValuePair.Value]);
                        }
                        else if (keyValuePair.Key.IsDefined(typeof(DataMemberAttribute)))
                        {
                            DataMemberAttribute attribute = keyValuePair.Key.GetCustomAttribute<DataMemberAttribute>();

                            switch (attribute.Type)
                            {
                                case DataType.Value:
                                    keyValuePair.Key.SetValue(entity, reader[keyValuePair.Value]);
                                    break;
                                case DataType.Entity:
                                    object repo = typeof(MySqlRepository<T, ID>).GetMethod("ResolveRepository")
                                        .MakeGenericMethod(keyValuePair.Key.PropertyType, attribute.RawType)
                                        .Invoke(this, new object[] {});
                                    object value = repo.GetType().GetMethod("FindOne")
                                        .Invoke(repo, new object[] {reader[keyValuePair.Value]});
                                    keyValuePair.Key.SetValue(entity, value);
                                    break;
                            }
                        }
                    }

                    yield return entity;
                }
            }
        }

        public IRepository<T, ID> ResolveRepository<T, ID>() where T : new()
        {
            return Injector.Resolve<IRepository<T, ID>>();
        }
    }
}