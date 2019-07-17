using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PostMasterWeb
{
    public class Post
    {
        [Key]
        public int Id { get; set; } = 0;
        public string Content { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }

        public bool IsValidated()
        {
            if (string.IsNullOrEmpty(Content) || CategoryId > 0 || UserId > 0)
            {
                return false;
            }

            return true;
        }
    }
}
