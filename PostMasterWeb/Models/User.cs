using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostMasterWeb
{
    public class User
    {
        [Key]
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public virtual List<Post> Posts { get; set; }

        public bool IsValidated()
        {
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Phone) || String.IsNullOrEmpty(Password))
            {
                return false;
            }

            return true;
        }
    }
}
