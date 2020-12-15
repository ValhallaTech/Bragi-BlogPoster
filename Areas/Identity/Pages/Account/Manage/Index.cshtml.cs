using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BragiBlogPoster.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BragiBlogPoster.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<BlogUser>   userManager;
        private readonly SignInManager<BlogUser> signInManager;

        public IndexModel(
            UserManager<BlogUser>   userManager,
            SignInManager<BlogUser> signInManager)
        {
            this.userManager   = userManager;
            this.signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(BlogUser user)
        {
            string userName    = await this.userManager.GetUserNameAsync(user).ConfigureAwait( false );
            string phoneNumber = await this.userManager.GetPhoneNumberAsync(user).ConfigureAwait( false );

            this.Username = userName;

            this.Input = new InputModel
                         {
                             PhoneNumber = phoneNumber
                         };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            BlogUser user = await this.userManager.GetUserAsync( this.User).ConfigureAwait( false );
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId( this.User)}'.");
            }

            await this.LoadAsync(user).ConfigureAwait( false );
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            BlogUser user = await this.userManager.GetUserAsync( this.User).ConfigureAwait( false );
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId( this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user).ConfigureAwait( false );
                return this.Page();
            }

            string phoneNumber = await this.userManager.GetPhoneNumberAsync(user).ConfigureAwait( false );
            if ( this.Input.PhoneNumber != phoneNumber)
            {
                IdentityResult setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber).ConfigureAwait( false );
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            await this.signInManager.RefreshSignInAsync(user).ConfigureAwait( false );
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }
    }
}
