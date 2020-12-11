using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BragiBlogPoster.Areas.Identity.Pages.Account.Manage
{
    public class GenerateRecoveryCodesModel : PageModel
    {
        private readonly UserManager<IdentityUser>           _userManager;
        private readonly ILogger<GenerateRecoveryCodesModel> _logger;

        public GenerateRecoveryCodesModel(
            UserManager<IdentityUser>           userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        {
            this._userManager = userManager;
            this._logger         = logger;
        }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IdentityUser user = await this._userManager.GetUserAsync( this.User).ConfigureAwait( false );
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId( this.User)}'.");
            }

            bool isTwoFactorEnabled = await this._userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait( false );
            if (!isTwoFactorEnabled)
            {
                string userId = await this._userManager.GetUserIdAsync(user).ConfigureAwait( false );
                throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' because they do not have 2FA enabled.");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IdentityUser user = await this._userManager.GetUserAsync( this.User).ConfigureAwait( false );
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId( this.User)}'.");
            }

            bool isTwoFactorEnabled = await this._userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait( false );
            string userId             = await this._userManager.GetUserIdAsync(user).ConfigureAwait( false );
            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' as they do not have 2FA enabled.");
            }

            IEnumerable<string> recoveryCodes = await this._userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait( false );
            this.RecoveryCodes = recoveryCodes.ToArray();

            this._logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);
            this.StatusMessage = "You have generated new recovery codes.";
            return this.RedirectToPage("./ShowRecoveryCodes");
        }
    }
}