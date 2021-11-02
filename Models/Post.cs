using System.ComponentModel.DataAnnotations;

namespace Blogs.Models
{
    public class Post
    {
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        //FK for what blog the given post is associated with
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}