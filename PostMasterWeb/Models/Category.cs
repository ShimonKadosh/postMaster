using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostMasterWeb
{
    public class Category
    {
        [Key]
        public int Id { get; set; } = 0;
        public string Name { get; set; }
    }
}
