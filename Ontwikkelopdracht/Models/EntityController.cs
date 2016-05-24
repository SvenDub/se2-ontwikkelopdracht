using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Internationalization]
    public abstract class EntityController<T> : Controller where T : new()
    {
        protected readonly IRepository<T> Repository = Injector.Resolve<IRepository<T>>();
    }
}