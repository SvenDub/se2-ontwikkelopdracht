using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Inject;
using Ontwikkelopdracht.Persistence;
using Ontwikkelopdracht.Persistence.MySql;
using Util;

namespace Ontwikkelopdracht
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            InjectDatabase();
        }

        private void InjectDatabase()
        {
            Injector.Register<IMySqlConnectionParams, ProductionMySqlConnectionParams>();

            AppDomain.CurrentDomain.GetAssemblies()
                // Get all entities with Entity and Identity attribute
                .SelectMany(x => x.GetTypes()
                        .Where(t => t.IsDefined(typeof(EntityAttribute), true))
                        .Where(t => t.GetProperties()
                            .Any(info => info.IsDefined(typeof(IdentityAttribute), true)))
                ).ToList().ForEach(t =>
                {
                    Log.I("DISCOVERY", $"Found entity '{t.Name}'");

                    typeof(MvcApplication).GetMethod("RegisterRepository")
                        .MakeGenericMethod(t)
                        .Invoke(this, new object[] {});
                });
        }

        public void RegisterRepository<T>() where T : new()
        {
            Injector.Register<IRepository<T>, MySqlRepository<T>>();
        }
    }
}