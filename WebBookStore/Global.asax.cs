using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebBookStore.Data;
using WebBookStore.Models;

namespace WebBookStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<StoreDbContext>());

            using (var context = new StoreDbContext())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Initialize(force: true);
                }
                else if (!context.Books.Any())
                {
                    // Nếu database tồn tại nhưng trống dữ liệu -> khởi tạo dữ liệu mặc định qua initializer
                    // DbInitializer.Seed(context) là protected trong DropCreateDatabaseIfModelChanges
                }
            }
        }
    }
}
