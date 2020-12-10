using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BragirBlogPoster.Models
{
    public class Post
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public string Title { get; set; }

        public string Abstract { get; set; }

        public string Content { get; set; }

        public string Slug { get; set; }

        [Display( Name = "FileName" )]
        public string FileName { get; set; }

        public byte[] Image { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public bool IsPublished { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
