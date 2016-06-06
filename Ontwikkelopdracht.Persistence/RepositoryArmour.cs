using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inject;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Persistence
{
    public class RepositoryArmour<T> : IRepository<T> where T : new()
    {
        private const string Tag = "REPOARMOUR";

        protected IStrictRepository<T> Repository = Injector.Resolve<IStrictRepository<T>>();

        private readonly PropertyInfo _identityProperty;

        public RepositoryArmour()
        {
            MemberInfo info = typeof(T);

            if (!info.GetCustomAttributes(true).Any(attr => attr is EntityAttribute))
            {
                EntityException exception = new EntityException($"Type {typeof(T)} is not attributed with Entity");
                ThrowInvalidCreate(exception);
            }

            PropertyInfo[] properties = typeof(T).GetProperties();

            if (!properties.Any(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute))))
            {
                EntityException exception =
                    new EntityException($"Type {typeof(T)} has no property attributed with Identity");
                ThrowInvalidCreate(exception);
            }

            _identityProperty = properties
                .First(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute)));
        }

        public long Count()
        {
            return Repository.Count();
        }

        public void Delete(int id)
        {
            if (id < 0)
            {
                ArgumentOutOfRangeException exception = new ArgumentOutOfRangeException(nameof(id), id,
                    "Valid values are >= 0.");
                ThrowInvalidMethodCall(exception);
            }

            Repository.Delete(id);
        }

        public void Delete(List<T> entities)
        {
            if (entities == null)
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(entities));
                ThrowInvalidMethodCall(exception);
            }
            if (entities.Any(entity => entity == null))
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(entities),
                    "The list contains a null reference.");
                ThrowInvalidMethodCall(exception);
            }
            entities.ForEach(entity =>
            {
                int id = (int) _identityProperty.GetValue(entity);
                if (id < 0)
                {
                    ArgumentOutOfRangeException exception =
                        new ArgumentOutOfRangeException(typeof(T).Name + "." + _identityProperty.Name, id,
                            "Valid values are >= 0.");
                    ThrowInvalidMethodCall(exception);
                }
            });

            Repository.Delete(entities);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(entity));
                ThrowInvalidMethodCall(exception);
            }
            int id = (int) _identityProperty.GetValue(entity);
            if (id < 0)
            {
                ArgumentOutOfRangeException exception =
                    new ArgumentOutOfRangeException(typeof(T).Name + "." + _identityProperty.Name, id,
                        "Valid values are >= 0.");
                ThrowInvalidMethodCall(exception);
            }

            Repository.Delete(entity);
        }

        public void DeleteAll()
        {
            Repository.DeleteAll();
        }

        public bool Exists(int id)
        {
            if (id < 0)
            {
                ArgumentOutOfRangeException exception = new ArgumentOutOfRangeException(nameof(id), id,
                    "Valid values are x >= 0.");
                ThrowInvalidMethodCall(exception);
            }

            return Repository.Exists(id);
        }

        public List<T> FindAll()
        {
            return Repository.FindAll();
        }

        public List<T> FindAll(List<int> ids)
        {
            if (ids == null)
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(ids));
                ThrowInvalidMethodCall(exception);
            }

            return Repository.FindAll(ids);
        }

        public List<T> FindAllWhere(Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(predicate));
                ThrowInvalidMethodCall(exception);
            }

            return Repository.FindAllWhere(predicate);
        }

        public List<T> FindAllWhere(Func<T, int, bool> predicate)
        {
            if (predicate == null)
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(predicate));
                ThrowInvalidMethodCall(exception);
            }

            return Repository.FindAllWhere(predicate);
        }

        public T FindOne(int id)
        {
            if (id < 0)
            {
                ArgumentOutOfRangeException exception = new ArgumentOutOfRangeException(nameof(id), id,
                    "Valid values are x >= 0.");
                ThrowInvalidMethodCall(exception);
            }

            return Repository.FindOne(id);
        }

        public T Save(T entity)
        {
            if (entity == null)
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(entity));
                ThrowInvalidMethodCall(exception);
            }
            int id = (int) _identityProperty.GetValue(entity);
            if (id < 0)
            {
                ArgumentOutOfRangeException exception =
                    new ArgumentOutOfRangeException(typeof(T).Name + "." + _identityProperty.Name, id,
                        "Valid values are >= 0.");
                ThrowInvalidMethodCall(exception);
            }

            return Repository.Save(entity);
        }

        public List<T> Save(List<T> entities)
        {
            if (entities == null)
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(entities));
                ThrowInvalidMethodCall(exception);
            }
            if (entities.Any(entity => entity == null))
            {
                ArgumentNullException exception = new ArgumentNullException(nameof(entities),
                    "The list contains a null reference.");
                ThrowInvalidMethodCall(exception);
            }
            entities.ForEach(entity =>
            {
                int id = (int) _identityProperty.GetValue(entity);
                if (id < 0)
                {
                    ArgumentOutOfRangeException exception =
                        new ArgumentOutOfRangeException(typeof(T).Name + "." + _identityProperty.Name, id,
                            "Valid values are >= 0.");
                    ThrowInvalidMethodCall(exception);
                }
            });

            return Repository.Save(entities);
        }

        private static void ThrowInvalidMethodCall(System.Exception exception)
        {
            Throw(exception, "Invalid method call.");
        }

        private static void ThrowInvalidCreate(System.Exception exception)
        {
            Throw(exception, "Invalid repository created.");
        }

        private static void Throw(System.Exception exception, string message)
        {
            Log.E(Tag, $"{message} {exception.Message}");
            throw exception;
        }
    }
}