using Books_Shop.Helpers;
using Books_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Books_Shop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppDbContext appDbContext = new AppDbContext();
            appDbContext.Database.CreateIfNotExists();
            var adminExist = appDbContext.Users.FirstOrDefault(s => s.UserName == "admin" && s.Code == "Admin2020");
            if (adminExist == null)
            {
                User adminUser = new User
                {
                    UserName = "admin",
                    Code = "Admin2020",
                    Password = Hashing.getHash("admin123")
                };
                appDbContext.Users.Add(adminUser);
                appDbContext.SaveChanges();
            }


            var booksExist = appDbContext.Books.ToList().FirstOrDefault();
            if (booksExist == null)
            {
                List<Book> books = new List<Book>()
                {
                    new Book()
                    {
                        Name = "C++",
                        Code = "1",
                        TotalEditions = 5,
                        CurrentEditions = 5
                    },

                    new Book(){

                        Name = "C#",
                        Code = "2",
                        TotalEditions = 4,
                        CurrentEditions = 4

                    },
                        new Book(){

                        Name = "Python",
                        Code = "3",
                        TotalEditions = 6,
                        CurrentEditions = 6

                    },
                            new Book(){

                        Name = "Java",
                        Code = "4",
                        TotalEditions = 3,
                        CurrentEditions = 3

                    }


                };

                appDbContext.Books.AddRange(books);
                appDbContext.SaveChanges();
            }
          
        }
    }
}
