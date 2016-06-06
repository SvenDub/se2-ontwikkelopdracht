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
            Repository.Delete(id);
        }

        public void Delete(List<T> entities)
        {
            Repository.Delete(entities);
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public void DeleteAll()
        {
            Repository.DeleteAll();
        }

        public bool Exists(int id)
        {
            return Repository.Exists(id);
        }

        public List<T> FindAll()
        {
            return Repository.FindAll();
        }

        public List<T> FindAll(List<int> ids)
        {
            return Repository.FindAll(ids);
        }

        public List<T> FindAllWhere(Func<T, bool> predicate)
        {
            return Repository.FindAllWhere(predicate);
        }

        public List<T> FindAllWhere(Func<T, int, bool> predicate)
        {
            return Repository.FindAllWhere(predicate);
        }

        public T FindOne(int id)
        {
            return Repository.FindOne(id);
        }

        public T Save(T entity)
        {
            return Repository.Save(entity);
        }

        public List<T> Save(List<T> entities)
        {
            return Repository.Save(entities);
        }
    }
}