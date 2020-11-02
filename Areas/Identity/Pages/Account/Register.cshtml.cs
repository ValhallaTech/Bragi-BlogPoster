using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BlogPosts.Models;

namespace BlogPosts.Areas.Identity.Pages.Account
{
    [AllowAnonymous] public class RegisterModel : PageModel
    {
        private readonly SignInManager<BlogUser> signInManager;
        private readonly UserManager<BlogUser>   userManager;
        private readonly ILogger<RegisterModel>  logger;
        private readonly IEmailSender            emailSender;

        public RegisterModel(
            UserManager<BlogUser>   userManager,
            SignInManager<BlogUser> signInManager,
            ILogger<RegisterModel>  logger,
            IEmailSender            emailSender)
        {
            this.userManager   = userManager;
            this.signInManager = signInManager;
            this.logger        = logger;
            this.emailSender   = emailSender;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                          MinimumLength     = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                          MinimumLength    = 2)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                          MinimumLength    = 2)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Display Name")] public string DisplayName { get; set; }

            [DataType(DataType.Password)]
            [Display(Name                     = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl      = returnUrl;
            ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl      = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new BlogUser
                           {
                               FirstName   = Input.FirstName,
                               LastName    = Input.LastName,
                               DisplayName = Input.DisplayName,
                               UserName    = Input.Email,
                               Email       = Input.Email
                           };

                var result = await this.userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                                               "/Account/ConfirmEmail",
                                               pageHandler: null,
                                               values: new
                                                       {
                                                           area      = "Identity", userId = user.Id, code = code,
                                                           returnUrl = returnUrl
                                                       },
                                               protocol: Request.Scheme);

                    await this.emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                                          $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new {email = Input.Email, returnUrl = returnUrl});
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}