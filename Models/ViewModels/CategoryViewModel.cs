using System.Collections.Generic;

namespace BragirBlogPoster.Models.ViewModels
{
    public class CategoryViewModel
    {
        public ICollection<Blog> Blogs { get; set; }

        // The entire Post model and all of its information
        public ICollection<Post> Posts { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public int PageNum { get; set; }

        public int TotalPosts { get; set; }
    }
}
