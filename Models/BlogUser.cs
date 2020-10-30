using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BlogPosts.Models
{
    public class BlogUser : IdentityUser
    {
        [Required] [StringLength(50)] public string FirstName   { get; set; }
        [Required] [StringLength(50)] public string LastName    { get; set; }
        public                               string DisplayName { get; set; }
    }
}