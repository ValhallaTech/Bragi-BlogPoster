using System;
using System.Collections.Generic;

namespace BlogPosts.Models
{
    public class Post
    {
        public int Id { get; set; }

        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }
        public int BlogId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public Blog Blog { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }

    }
}
