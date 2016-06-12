using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
using Ontwikkelopdracht.Persistence.Exception;
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

            PersistenceInjector.Inject(new MySqlRepositoryProvider());

            IRepository<MetaKeyValuePair> metaRepository = Injector.Resolve<IRepository<MetaKeyValuePair>>();

            bool setupSuccess = true;

            try
            {
                List<MetaKeyValuePair> metaKeyValuePairs = metaRepository.FindAllWhere(pair => pair.Key == "DB_VERSION");

                if (metaKeyValuePairs.Count == 0)
                {
                    Log.W("DB", "Could not find META.DB_VERSION. Database not properly initialized.");
                    setupSuccess = Injector.Resolve<IRepositorySetup>().Setup();
                }
            }
            catch (DataSourceException e)
            {
                Log.W("DB", "Database not initialized.");
                setupSuccess = Injector.Resolve<IRepositorySetup>().Setup();
            }

            if (!setupSuccess)
            {
                Environment.Exit(13);
            }
        }
    }
}