using System.Text;
using System.Threading.Tasks;
using BragiBlogPoster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BragiBlogPoster.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<BlogUser> userManager;
        private readonly IEmailSender          sender;

        public RegisterConfirmationModel(UserManager<BlogUser> userManager, IEmailSender sender)
        {
            this.userManager = userManager;
            this.sender      = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if ( email == null )
            {
                return this.RedirectToPage( "/Index" );
            }

            BlogUser user = await this.userManager.FindByEmailAsync( email ).ConfigureAwait( false );

            if ( user == null )
            {
                return this.NotFound( $"Unable to load user with email '{email}'." );
            }

            this.Email = email;

            // Once you add a real email sender, you should remove this code that lets you confirm the account
            this.DisplayConfirmAccountLink = true;

            if ( this.DisplayConfirmAccountLink )
            {
                string userId = await this.userManager.GetUserIdAsync( user ).ConfigureAwait( false );
                string code   = await this.userManager.GenerateEmailConfirmationTokenAsync( user ).ConfigureAwait( false );
                code = WebEncoders.Base64UrlEncode( Encoding.UTF8.GetBytes( code ) );
                this.EmailConfirmationUrl = this.Url.Page(
                                                          "/Account/ConfirmEmail",
                                                          pageHandler: null,
                                                          values: new
                                                                  {
                                                                      area      = "Identity",
                                                                      userId    = userId,
                                                                      code      = code,
                                                                      returnUrl = returnUrl
                                                                  },
                                                          protocol: this.Request.Scheme );
            }

            return this.Page( );
        }
    }
}
