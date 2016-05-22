using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    public abstract class EntityController<T, ID> : Controller where T : new()
    {
        protected readonly IRepository<T, ID> Repository = Injector.Resolve<IRepository<T, ID>>();
    }
}