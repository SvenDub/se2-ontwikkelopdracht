using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    public abstract class EntityController<T> : Controller
    {
        protected IRepository<T, int> Repository = Injector.Resolve<IRepository<T, int>>();
    }
}