using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inject;
using Ontwikkelopdracht.Persistence.Exception;

namespace Ontwikkelopdracht.Persistence
{
    public class RepositoryArmour<T> : IRepository<T> where T : new()
    {
        protected IStrictRepository<T> Repository = Injector.Resolve<IStrictRepository<T>>();

        public RepositoryArmour()
        {
            MemberInfo info = typeof(T);

            if (!info.GetCustomAttributes(true).Any(attr => attr is EntityAttribute))
            {
                throw new EntityException($"Type {typeof(T)} is not attributed with Entity");
            }

            PropertyInfo[] properties = typeof(T).GetProperties();

            if (!properties.Any(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute))))
            {
                throw new EntityException($"Type {typeof(T)} has no property attributed with Identity");
            }
        }

        public long Count()
        {
            return Repository.Count();
        }

        public void Delete(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, "Valid values are x >= 0.");
            }

            Repository.Delete(id);
        }

        public void Delete(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (entities.Any(entity => entity == null))
            {
                throw new ArgumentNullException(nameof(entities), "The list contains a null reference.");
            }

            Repository.Delete(entities);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
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
                throw new ArgumentOutOfRangeException(nameof(id), id, "Valid values are x >= 0.");
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
                throw new ArgumentNullException(nameof(ids));
            }

            return Repository.FindAll(ids);
        }

        public List<T> FindAllWhere(Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return Repository.FindAllWhere(predicate);
        }

        public List<T> FindAllWhere(Func<T, int, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return Repository.FindAllWhere(predicate);
        }

        public T FindOne(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, "Valid values are x >= 0.");
            }

            return Repository.FindOne(id);
        }

        public T Save(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return Repository.Save(entity);
        }

        public List<T> Save(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (entities.Any(entity => entity == null))
            {
                throw new ArgumentNullException(nameof(entities), "The list contains a null reference.");
            }

            return Repository.Save(entities);
        }
    }
}