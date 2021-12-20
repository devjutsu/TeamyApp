// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Teamy.Server.Models;
using Teamy.Server.Data;

namespace Teamy.Server.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly TeamyDbContext _db;

        public LoginModel(SignInManager<AppUser> signInManager, ILogger<LoginModel> logger, TeamyDbContext db)
        {
            _signInManager = signInManager;
            _logger = logger;
            _db = db;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            //Anonymous Participation
            [DataType(DataType.Text)]
            public string Participation { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null, string participation = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            Input = new InputModel() { Participation = participation };

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    HttpContext.Request.Cookies.TryGetValue("userId", out string cookieUserId);
                    if(cookieUserId != null)
                    {
                        var currentUserId = _db.Users.Where(o => o.Email == Input.Email).SingleOrDefault().Id;
                        if(cookieUserId != currentUserId)
                        {
                            var createdEvents = _db.Events.Where(e => e.CreatedById == cookieUserId);
                            if(createdEvents?.Any() ?? false)
                            {
                                foreach(var e in createdEvents)
                                {
                                    // @! make sure this event belongs to logged in user (and it's totally fresh event)

                                    e.CreatedById = currentUserId;
                                }
                                _db.Events.UpdateRange(createdEvents);
                                _db.SaveChanges();
                            }
                        }

                        // @! logic delete cookie user and write down his currentUserId

                        HttpContext.Response.Cookies.Delete(cookieUserId);
                        HttpContext.Response.Cookies.Append("userId", cookieUserId, new CookieOptions() { Expires = DateTime.Now.AddYears(-1) });
                    }

                    if (!string.IsNullOrEmpty(Input.Participation))
                    {
                        var anonymousParticipation = _db.AnonParticipation.Find(Guid.Parse(Input.Participation));
                        var currentUserId = _db.Users.Where(o => o.Email == Input.Email).SingleOrDefault().Id;

                        if(anonymousParticipation != null)
                        {
                            var existingAnswers = _db.Participation.Where(o => o.EventId == anonymousParticipation.EventId && o.UserId == currentUserId);
                            if(existingAnswers.Any())
                                _db.Participation.RemoveRange(existingAnswers);

                            var realParticipation = new Participation()
                            {
                                EventId = anonymousParticipation.EventId,
                                InviteId = anonymousParticipation.InviteId,
                                Status = anonymousParticipation.Status,
                                UserId = currentUserId,
                            };

                            _db.Participation.Add(realParticipation);
                            _db.AnonParticipation.Remove(anonymousParticipation);
                            _db.SaveChanges();

                            returnUrl = $"/event/{realParticipation.EventId}";
                        }
                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
