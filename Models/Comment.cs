using System;

namespace BragiBlogPoster.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string BlogUserId { get; set; }

        public string Content { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        public Post Post { get; set; }

        public virtual BlogUser BlogUser { get; set; }
    }
}
