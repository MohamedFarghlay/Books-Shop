using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Books_Shop.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
          : base(@"Data Source =.\SQLEXPRESS; Initial Catalog =BooksShop_DB; Trusted_Connection=True;MultipleActiveResultSets=true")
        {

        }
        //Books Table
        public DbSet<Book> Books { get; set; }

        //Users Table
        public DbSet <User> Users { get; set; }

        //UserBooks Table
        public DbSet<UserBooks> UserBooks { get; set; }
    }

        

}