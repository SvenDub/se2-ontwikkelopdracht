using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
using Ontwikkelopdracht.Persistence.MySql;

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
                    Type propertyType =
                        t.GetProperties()
                            .First(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute), true))
                            .PropertyType;

                    Log.I("DISCOVERY", $"Found entity '{t.Name}' with ID type '{propertyType.Name}'");

                    typeof(MvcApplication).GetMethod("RegisterRepository")
                        .MakeGenericMethod(t, propertyType)
                        .Invoke(this, new object[] {});
                });
        }

        public void RegisterRepository<T, ID>() where T : new()
        {
            Injector.Register<IRepository<T, ID>, MySqlRepository<T, ID>>();
        }
    }
}