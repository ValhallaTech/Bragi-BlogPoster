using System;

namespace BlogPosts.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string AuthorId { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public Post Post { get; set; }

        public virtual BlogUser Author { get; set; }
    }
}
