using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BragiBlogPoster.Models
{
    public class BlogUser : IdentityUser
    {
        [Required] [StringLength( 50 )] public string FirstName { get; set; }

        [Required] [StringLength( 50 )] public string LastName { get; set; }

        public string DisplayName { get; set; }
    }
}
