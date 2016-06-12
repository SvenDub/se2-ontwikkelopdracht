using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inject;
using MySql.Data.MySqlClient;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Persistence.MySql
{
    /// <summary>
    ///     Repository that connects to a MySQL backend.
    /// </summary>
    public class MySqlRepository<T> : IStrictRepository<T> where T : new()
    {
        /// <summary>
        ///     Connection parameters to use.
        /// </summary>
        public IMySqlConnectionParams MySqlConnectionParams { set; protected get; } =
            Injector.Resolve<IMySqlConnectionParams>();

        private readonly EntityAttribute _entityAttribute;
        private readonly IdentityAttribute _identityAttribute;
        private readonly PropertyInfo _identityProperty;

        /// <summary>
        ///     Checks if the entity is valid.
        /// </summary>
        public MySqlRepository()
        {
            MemberInfo info = typeof(T);

            if (!info.GetCustomAttributes(true).Any(attr => attr is EntityAttribute))
            {
                throw new EntityException($"Type {typeof(T)} is not attributed with Entity");
            }

            _entityAttribute = info.GetCustomAttributes(true)
                .OfType<EntityAttribute>()
                .First();

            PropertyInfo[] properties = typeof(T).GetProperties();

            if (!properties.Any(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute))))
            {
                throw new EntityException($"Type {typeof(T)} has no property attributed with Identity");
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
            try
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
            catch (MySqlException e)
            {
                throw new DataSourceException(e);
            }
        }

        public void Delete(int id)
        {
            try
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
            catch (MySqlException e)
            {
                throw new DataSourceException(e);
            }
        }

        public void Delete(List<T> entities)
        {
            entities.ForEach(Delete);
        }

        public void Delete(T entity)
        {
            try
            {
                Delete((int) _identityProperty.GetGetMethod().Invoke(entity, new object[] {}));
            }
            catch (MySqlException e)
            {
                throw new DataSourceException(e);
            }
        }

        public void DeleteAll()
        {
            try
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
            catch (MySqlException e)
            {
                throw new DataSourceException(e);
            }
        }

        public bool Exists(int id)
        {
            try
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
            catch (MySqlException e)
            {
                throw new DataSourceException(e);
            }
        }

        public List<T> FindAll()
        {
            try
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
            catch (MySqlException e)
            {
                throw new DataSourceException(e);
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
            try
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
            catch (MySqlException e)
            {
                throw new DataSourceException(e);
            }
        }

        public T Save(T entity)
        {
            int id = (int) _identityProperty.GetValue(entity);
            try
            {
                return Exists(id) ? Update(entity, id) : Insert(entity);
            }
            catch (MySqlException e)
            {
                throw new DataSourceException(e);
            }
        }

        /// <summary>
        ///     Update an entity with the given id.
        /// </summary>
        /// <param name="entity">The new entity values.</param>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The saved entity.</returns>
        private T Update(T entity, int id)
        {
            using (MySqlConnection connection = CreateConnection())
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    string[] parameters = new string[DataMembersWithoutIdentity.Count];
                    int i = -1;
                    foreach (KeyValuePair<PropertyInfo, string> keyValuePair in DataMembersWithoutIdentity)
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

                    T saved = FindOne(id);
                    SaveOneToMany(entity, saved, id);
                    return saved;
                }
            }
        }

        /// <summary>
        ///     Insert a new entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The saved entity.</returns>
        private T Insert(T entity)
        {
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

                    T saved = FindOne((int) cmd.LastInsertedId);
                    SaveOneToMany(entity, saved, (int) cmd.LastInsertedId);
                    return saved;
                }
            }
        }

        /// <summary>
        ///     Save all <see cref="DataType.OneToManyEntity"/> properties after the entity itself has been saved.
        ///     The new id has to be known at this point.
        /// </summary>
        /// <param name="entity">The old entity.</param>
        /// <param name="saved">The saved entity.</param>
        /// <param name="id">The id of the saved entity.</param>
        /// <exception cref="EntityException">If <see cref="DataType.OneToManyEntity"/> was incorrectly defined.</exception>
        private void SaveOneToMany(T entity, T saved, int id)
        {
            DataMembersOneToMany.ForEach(key =>
            {
                if (key.PropertyType.IsGenericType && key.PropertyType.GetGenericTypeDefinition()
                    == typeof(List<>))
                {
                    Type itemType = key.PropertyType.GetGenericArguments()[0];

                    // Save one to many entities to their repo
                    object repo = typeof(MySqlRepository<T>).GetMethod("ResolveRepository")
                        .MakeGenericMethod(itemType)
                        .Invoke(this, new object[] {});

                    IList entities = (IList) key.GetValue(entity);
                    entities.Cast<object>().ToList().ForEach(e =>
                    {
                        e.GetType()
                            .GetProperties()
                            .Where(
                                info =>
                                    info.IsDefined(typeof(DataMemberAttribute)) &&
                                    info.GetCustomAttribute<DataMemberAttribute>().RawType == typeof(T))
                            .ToList()
                            .ForEach(info => info.SetValue(e, id));
                    });

                    object savedValues = repo.GetType().GetMethod("Save", new[] {key.PropertyType})
                        .Invoke(repo, new object[] {entities});

                    // Set entities to new object
                    key.SetValue(saved, savedValues);
                }
                else
                {
                    throw new EntityException("DataType.OneToMany can only be defined on a List<>.");
                }
            });
        }

        public List<T> Save(List<T> entities)
        {
            return entities.Select(Save).ToList();
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
        ///     All properties attributed with <see cref="DataMemberAttribute"/> and their Column name.
        /// </summary>
        private Dictionary<PropertyInfo, string> DataMembers => typeof(T)
            .GetProperties()
            .Where(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute), true))
            .Select(
                propertyInfo =>
                    new KeyValuePair<PropertyInfo, string>(propertyInfo,
                        propertyInfo.GetCustomAttribute<IdentityAttribute>(true).Column))
            .Concat(DataMembersWithoutIdentity)
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        /// <summary>
        ///     All properties attributed with <see cref="DataMemberAttribute"/> except <see cref="IdentityAttribute"/> and their Column name.
        /// </summary>
        private Dictionary<PropertyInfo, string> DataMembersWithoutIdentity => typeof(T)
            .GetProperties()
            .Where(
                propertyInfo =>
                    propertyInfo.IsDefined(typeof(DataMemberAttribute), true) &&
                    propertyInfo.GetCustomAttribute<DataMemberAttribute>().Type != DataType.OneToManyEntity)
            .Select(
                propertyInfo =>
                    new KeyValuePair<PropertyInfo, string>(propertyInfo,
                        propertyInfo.GetCustomAttribute<DataMemberAttribute>(true).Column))
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        /// <summary>
        ///     All properties attributed with <see cref="DataType.OneToManyEntity"/>.
        /// </summary>
        private List<PropertyInfo> DataMembersOneToMany => typeof(T)
            .GetProperties()
            .Where(
                propertyInfo =>
                    propertyInfo.IsDefined(typeof(DataMemberAttribute), true) &&
                    propertyInfo.GetCustomAttribute<DataMemberAttribute>().Type == DataType.OneToManyEntity)
            .ToList();

        /// <summary>
        ///     Create a new entity from the given reader.
        /// </summary>
        /// <param name="reader">The reader that contains the data.</param>
        /// <returns>A new entity.</returns>
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

        /// <summary>
        ///     Create a new list of entities from the given reader.
        /// </summary>
        /// <param name="reader">The reader that contains the data.</param>
        /// <returns>A new list of entities.</returns>
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
        ///     Create a new entity from the given reader.
        ///     Expects reader to contain data at current row. Does not clean up reader.
        /// </summary>
        /// <param name="reader">The reader that contains the data.</param>
        /// <returns>A new entity.</returns>
        /// <exception cref="EntityException">When a property can not be populated.</exception>
        private T CreateFromRow(MySqlDataReader reader)
        {
            try
            {
                T entity = new T();
                foreach (var keyValuePair in DataMembers)
                {
                    // Do not set null values
                    if (reader.IsDBNull(reader.GetOrdinal(keyValuePair.Value))) continue;

                    // Set the Identity
                    if (keyValuePair.Key.IsDefined(typeof(IdentityAttribute)))
                    {
                        keyValuePair.Key.SetValue(entity, reader[keyValuePair.Value]);
                    }
                    // Set DataMember
                    else if (keyValuePair.Key.IsDefined(typeof(DataMemberAttribute)))
                    {
                        DataMemberAttribute attribute = keyValuePair.Key.GetCustomAttribute<DataMemberAttribute>();

                        switch (attribute.Type)
                        {
                            // Set raw value. Perform conversion if necessary and supported.
                            case DataType.Value:
                                // Boolean
                                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                                if (keyValuePair.Key.PropertyType == typeof(bool))
                                {
                                    keyValuePair.Key.SetValue(entity, Convert.ToBoolean(reader[keyValuePair.Value]));
                                }
                                // Generic
                                else
                                {
                                    keyValuePair.Key.SetValue(entity, reader[keyValuePair.Value]);
                                }
                                break;
                            // Resolve entity from other IRepository
                            case DataType.Entity:
                                object repo = typeof(MySqlRepository<T>).GetMethod("ResolveRepository")
                                    .MakeGenericMethod(keyValuePair.Key.PropertyType)
                                    .Invoke(this, new object[] {});
                                object value = repo.GetType().GetMethod("FindOne")
                                    .Invoke(repo, new object[] {reader[keyValuePair.Value]});
                                keyValuePair.Key.SetValue(entity, value);
                                break;
                            // Resolve one to many entities
                            case DataType.OneToManyEntity:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                return entity;
            }
            catch (ArgumentException e)
            {
                throw new EntityException(
                    "The data type does not match and conversion is not yet implemented for this type.", e);
            }
        }

        /// <summary>
        ///     Add parameters to insert statement.
        /// </summary>
        /// <param name="cmd">The statement to which the parameters should be added.</param>
        /// <param name="entity">The entity to insert.</param>
        /// <exception cref="EntityException">When a property can not be populated.</exception>
        private void AddInsertParametersForEntity(MySqlCommand cmd, T entity)
        {
            try
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
                        case DataType.OneToManyEntity:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (ArgumentException e)
            {
                throw new EntityException(
                    "The data type does not match and conversion is not yet implemented for this type.", e);
            }
        }

        /// <summary>
        ///     Add parameters to update statement.
        /// </summary>
        /// <param name="cmd">The statement to which the parameters should be added.</param>
        /// <param name="entity">The entity to insert.</param>
        /// <exception cref="EntityException">When a property can not be populated.</exception>
        private void AddUpdateParametersForEntity(MySqlCommand cmd, T entity)
        {
            try
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
                            case DataType.OneToManyEntity:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                throw new EntityException(
                    "The data type does not match and conversion is not yet implemented for this type.", e);
            }
        }

        /// <summary>
        ///     Resolve the repository for the given entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity to resolve.</typeparam>
        /// <returns>The repository for the given entity.</returns>
        public IRepository<TEntity> ResolveRepository<TEntity>() where TEntity : new()
        {
            return Injector.Resolve<IRepository<TEntity>>();
        }
    }
}