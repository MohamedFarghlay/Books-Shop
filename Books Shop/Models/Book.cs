using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Books_Shop.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public int TotalEditions { get; set; }
        public int CurrentEditions { get; set; }

        //relationship with users (many-to-many)
        public virtual ICollection<User> users { get; set; }


    }
}