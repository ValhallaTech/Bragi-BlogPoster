using System.Collections.Generic;

namespace BragirBlogPoster.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        protected virtual ICollection<Post> Posts { get; set; }

        public Blog( )
        {
            this.Posts = new HashSet<Post>( );
        }
    }
}
