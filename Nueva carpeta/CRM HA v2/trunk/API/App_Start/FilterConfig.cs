using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace MobileServices
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
        }
    }
}