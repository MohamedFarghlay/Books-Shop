using Books_Shop.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Books_Shop.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Code { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        //relationship with books (many-to-many)
        public virtual ICollection<Book> books { get; set; }

    }
}