using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPosts.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }
}
