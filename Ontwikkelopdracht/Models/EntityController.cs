using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    /// <summary>
    ///     Controller for a specific entity.
    /// </summary>
    [Internationalization]
    public abstract class EntityController<T> : Controller where T : new()
    {
        /// <summary>
        ///     The repository for entity <see cref="T"/>.
        /// </summary>
        protected readonly IRepository<T> Repository = Injector.Resolve<IRepository<T>>();
    }
}