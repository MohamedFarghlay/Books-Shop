using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Books_Shop.Models
{
    public class UserBooks
    {
        public int Id { get; set; }
        public virtual Book books { get; set; }
        public virtual User users { get; set; }
        public int NumberOfEditions { get; set; }
    }
}