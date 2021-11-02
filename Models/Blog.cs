using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blogs.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        [Required]
        public string Name { get; set; }

        //Return collection of Post associated with given blog
        public ICollection<Post> Posts { get; set; }
    }
}