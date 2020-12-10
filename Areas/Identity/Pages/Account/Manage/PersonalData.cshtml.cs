using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BragirBlogPoster.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser>  _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        public PersonalDataModel(
            UserManager<IdentityUser>  userManager,
            ILogger<PersonalDataModel> logger)
        {
            this._userManager = userManager;
            this._logger         = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            IdentityUser user = await this._userManager.GetUserAsync( this.User).ConfigureAwait( false );
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId( this.User)}'.");
            }

            return this.Page();
        }
    }
}