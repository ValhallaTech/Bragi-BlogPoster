﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BragirBlogPoster.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWithRecoveryCodeModel : PageModel
    {
        private readonly SignInManager<IdentityUser>         _signInManager;
        private readonly ILogger<LoginWithRecoveryCodeModel> _logger;

        public LoginWithRecoveryCodeModel(SignInManager<IdentityUser> signInManager, ILogger<LoginWithRecoveryCodeModel> logger)
        {
            this._signInManager = signInManager;
            this._logger           = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [BindProperty]
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            IdentityUser user = await this._signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait( false );
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            this.ReturnUrl = returnUrl;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            IdentityUser user = await this._signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait( false );
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            string recoveryCode = this.Input.RecoveryCode.Replace(" ", string.Empty);

            SignInResult result = await this._signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode).ConfigureAwait( false );

            if (result.Succeeded)
            {
                this._logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
                return this.LocalRedirect(returnUrl ?? this.Url.Content("~/"));
            }

            if (result.IsLockedOut)
            {
                this._logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                this._logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                this.ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return this.Page();
            }
        }
    }
}