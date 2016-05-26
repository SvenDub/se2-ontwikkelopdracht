using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inject;
using MySql.Data.MySqlClient;
using Util;

namespace Ontwikkelopdracht.Persistence.MySql
{
    public class MySqlRepository<T> : IRepository<T> where T : new()
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

        public void Delete(int id)
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM {_entityAttribute.Table} WHERE {_identityAttribute.Column}=@Id";
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(List<T> entities)
        {
            entities.ForEach(Delete);
        }

        public void Delete(T entity)
        {
            Delete((int) _identityProperty.GetGetMethod().Invoke(entity, new object[] {}));
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

        public bool Exists(int id)
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                        $"SELECT COUNT(*) FROM {_entityAttribute.Table} WHERE {_identityAttribute.Column}=@Id";
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

        public List<T> FindAll(List<int> ids)
        {
            return ids.Select(FindOne).ToList();
        }

        public List<T> FindAllWhere(Func<T, bool> predicate)
        {
            return FindAll().Where(predicate).ToList();
        }

        public List<T> FindAllWhere(Func<T, int, bool> predicate)
        {
            return FindAll().Where(predicate).ToList();
        }

        public T FindOne(int id)
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT `{string.Join("`, `", DataMembers.Values)}` " +
                                      $"FROM {_entityAttribute.Table} " +
                                      $"WHERE {_identityAttribute.Column}=@Id";
                    cmd.Parameters.AddWithValue("Id", id);

                    return CreateFromReader(cmd.ExecuteReader());
                }
            }
        }

        public T Save(T entity)
        {
            int id = (int) _identityProperty.GetValue(entity);
            if (Exists(id))
            {
                // Update
                using (MySqlConnection connection = CreateConnection())
                {
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        string[] parameters = new string[DataMembersWithoutIdentity.Count];
                        int i = -1;
                        foreach (var keyValuePair in DataMembersWithoutIdentity)
                        {
                            i++;
                            parameters[i] = $"`{keyValuePair.Value}`=@{keyValuePair.Value}";
                        }

                        cmd.CommandText =
                            $"UPDATE {_entityAttribute.Table} " +
                            $"SET {string.Join(", ", parameters)} " +
                            $"WHERE `{_identityAttribute.Column}`=@Id";

                        cmd.Parameters.AddWithValue("Id", id);

                        AddUpdateParametersForEntity(cmd, entity);


                        cmd.ExecuteNonQuery();
                        return FindOne(id);
                    }
                }
            }
            else
            {
                // Insert
                using (MySqlConnection connection = CreateConnection())
                {
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        string[] parameters = new string[DataMembersWithoutIdentity.Count];
                        int i = -1;
                        foreach (var keyValuePair in DataMembersWithoutIdentity)
                        {
                            i++;
                            parameters[i] = $"@{keyValuePair.Value}";
                        }

                        cmd.CommandText =
                            $"INSERT INTO {_entityAttribute.Table} (`{string.Join("`, `", DataMembersWithoutIdentity.Values)}`) " +
                            $"VALUES ({string.Join(", ", parameters)})";

                        AddInsertParametersForEntity(cmd, entity);

                        cmd.ExecuteNonQuery();
                        return FindOne((int) cmd.LastInsertedId);
                    }
                }
            }
        }

        public List<T> Save(List<T> entities)
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
            .Concat(DataMembersWithoutIdentity)
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        private Dictionary<PropertyInfo, string> DataMembersWithoutIdentity => typeof(T)
            .GetProperties()
            .Where(propertyInfo => propertyInfo.IsDefined(typeof(DataMemberAttribute), true))
            .Select(
                propertyInfo =>
                    new KeyValuePair<PropertyInfo, string>(propertyInfo,
                        propertyInfo.GetCustomAttribute<DataMemberAttribute>(true).Column))
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        private T CreateFromReader(MySqlDataReader reader)
        {
            using (reader)
            {
                while (reader.Read())
                {
                    return CreateFromRow(reader);
                }

                throw new EntityNotFoundException();
            }
        }

        private IEnumerable<T> CreateListFromReader(MySqlDataReader reader)
        {
            using (reader)
            {
                while (reader.Read())
                {
                    yield return CreateFromRow(reader);
                }
            }
        }

        /// <summary>
        ///     Expects reader to contain data at current row. Does not clean up reader.
        /// </summary>
        private T CreateFromRow(MySqlDataReader reader)
        {
            T entity = new T();
            foreach (var keyValuePair in DataMembers)
            {
                if (reader.IsDBNull(reader.GetOrdinal(keyValuePair.Value))) continue;

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
                            if (keyValuePair.Key.PropertyType == typeof(bool))
                            {
                                keyValuePair.Key.SetValue(entity, Convert.ToBoolean(reader[keyValuePair.Value]));
                            }
                            else
                            {
                                keyValuePair.Key.SetValue(entity, reader[keyValuePair.Value]);
                            }
                            break;
                        case DataType.Entity:
                            object repo = typeof(MySqlRepository<T>).GetMethod("ResolveRepository")
                                .MakeGenericMethod(keyValuePair.Key.PropertyType)
                                .Invoke(this, new object[] {});
                            object value = repo.GetType().GetMethod("FindOne")
                                .Invoke(repo, new object[] {reader[keyValuePair.Value]});
                            keyValuePair.Key.SetValue(entity, value);
                            break;
                    }
                }
            }
            return entity;
        }

        private void AddInsertParametersForEntity<TEntity>(MySqlCommand cmd, TEntity entity) where TEntity : T
        {
            foreach (var keyValuePair in DataMembersWithoutIdentity)
            {
                DataMemberAttribute attribute = keyValuePair.Key.GetCustomAttribute<DataMemberAttribute>();

                switch (attribute.Type)
                {
                    case DataType.Value:
                        cmd.Parameters.AddWithValue(attribute.Column, keyValuePair.Key.GetValue(entity));
                        break;
                    case DataType.Entity:
                        object repo = typeof(MySqlRepository<T>).GetMethod("ResolveRepository")
                            .MakeGenericMethod(keyValuePair.Key.PropertyType)
                            .Invoke(this, new object[] {});

                        object nestedEntity = repo.GetType()
                            .GetMethod("Save", new Type[] {keyValuePair.Key.PropertyType})
                            .Invoke(repo, new object[] {keyValuePair.Key.GetValue(entity)});

                        object nestedId = nestedEntity.GetType()
                            .GetProperties()
                            .First(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute)))
                            .GetValue(nestedEntity);
                        cmd.Parameters.AddWithValue(attribute.Column, nestedId);
                        break;
                }
            }
        }

        private void AddUpdateParametersForEntity<TEntity>(MySqlCommand cmd, TEntity entity)
        {
            foreach (var keyValuePair in DataMembers)
            {
                if (keyValuePair.Key.IsDefined(typeof(IdentityAttribute)))
                {
                    IdentityAttribute attribute =
                        keyValuePair.Key.GetCustomAttribute<IdentityAttribute>();

                    cmd.Parameters.AddWithValue(attribute.Column, keyValuePair.Key.GetValue(entity));
                }
                else if (keyValuePair.Key.IsDefined(typeof(DataMemberAttribute)))
                {
                    DataMemberAttribute attribute =
                        keyValuePair.Key.GetCustomAttribute<DataMemberAttribute>();

                    switch (attribute.Type)
                    {
                        case DataType.Value:
                            cmd.Parameters.AddWithValue(attribute.Column, keyValuePair.Key.GetValue(entity));
                            break;
                        case DataType.Entity:
                            object repo = typeof(MySqlRepository<T>).GetMethod("ResolveRepository")
                                .MakeGenericMethod(keyValuePair.Key.PropertyType)
                                .Invoke(this, new object[] {});
                            object nestedEntity = repo.GetType()
                                .GetMethod("Save", new Type[] {keyValuePair.Key.PropertyType})
                                .Invoke(repo, new object[] {keyValuePair.Key.GetValue(entity)});
                            object nestedId = nestedEntity.GetType()
                                .GetProperties()
                                .First(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute)))
                                .GetValue(nestedEntity);
                            cmd.Parameters.AddWithValue(attribute.Column, nestedId);
                            break;
                    }
                }
            }
        }

        public IRepository<TEntity> ResolveRepository<TEntity>() where TEntity : new()
        {
            return Injector.Resolve<IRepository<TEntity>>();
        }
    }
}