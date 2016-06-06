﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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

            PersistenceInjector.Inject(new MySqlRepositoryProvider());
        }
    }
}