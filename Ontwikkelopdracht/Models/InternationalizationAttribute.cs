﻿using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Ontwikkelopdracht.Models
{
    public class InternationalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string language = (string) filterContext.RouteData.Values["language"] ?? "nl";
            string culture = (string) filterContext.RouteData.Values["culture"] ?? "NL";

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo($"{language}-{culture}");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo($"{language}-{culture}");
        }
    }
}