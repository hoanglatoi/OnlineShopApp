using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OnlineShop.Data;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Repositories;
using OnlineShop.Model.Models;
using OnlineShop.Service.Services.Token;
using OnlineShop.Ulities;
using OnlineShop.ViewModels;

namespace OnlineShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ITokenManager _tokenManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationUserGroupRepository _applicationUserGroupRepository;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            ITokenManager tokenManager,
            IConfiguration configuration,
            IApplicationUserGroupRepository applicationUserGroupRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenManager = tokenManager;
            _configuration = configuration;
            _applicationUserGroupRepository = applicationUserGroupRepository;
        }

        [BindProperty]
        public InputModel? Input { get; set; }

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        public string? ReturnUrl { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        //
        // 概要:
        //     Validates an email address.
        [AttributeUsage(
            AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
            AllowMultiple = false)]
        public sealed class UserNameOrEmailAttribute : DataTypeAttribute
        {
            //
            // 概要:
            //     Initializes a new instance of the System.ComponentModel.DataAnnotations.EmailAddressAttribute
            //     class.
            public UserNameOrEmailAttribute() : base("UserNameOrEmail")
            {
                ;
            }

            //
            // 概要:
            //     Determines whether the specified value matches the pattern of a valid email address.
            //
            // パラメーター:
            //   value:
            //     The value to validate.
            //
            // 戻り値:
            //     true if the specified value is valid or null; otherwise, false.
            public override bool IsValid(object? value)
            {
                return true;
            }
        }

        public class InputModel
        {
            [Required]
            [UserNameOrEmail]
            [Display(Name = "UserName or Email")]
            public string? UNameOrEmail { get; set; }


            //[Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // e-mail形式で無くてもOKに
                var user = await _userManager.FindByNameAsync(Input?.UNameOrEmail);
                if (user != null)
                {
                    Input!.UNameOrEmail = user.UserName;
                }
                var userGroup = _applicationUserGroupRepository.GetSingleByCondition(x => x.UserId == user.Id);
                //if (userGroup == null)
                //{
                //    return BadRequest("Not found user in user_group table");
                //}

                if (userGroup != null && userGroup.GroupId != 2) // if user not customer
                {
                    //return BadRequest("Not found user");
                    return new RedirectToRouteResult(new { page = "/Account/Error", area = "Identity", title = "Error", errormsg = "Not-found-user" });
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var result = await _signInManager.PasswordSignInAsync(Input?.UNameOrEmail, Input?.Password, Input!.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    // Get group
                    var customerGroup = new ApplicationGroup
                    {
                        Name = "Customer",
                        ID = 2
                    };
                    
                    // Generate token
                    var accessToken = _tokenManager.GenerateAccessToken(user, customerGroup);
                    var refreshToken = _tokenManager.GenerateRefreshToken(user.UserName);
                    bool ret = await SetRefreshToken(refreshToken, user);

                    // Add to cookie
                    var cookieOptions = new CookieOptions
                    {
                        Expires = GetExpire(),
                        HttpOnly = _configuration.GetSection("CookieOptions:HttpOnly").Get<bool>(),
                        SameSite = GetSameSite(),
                        Secure = _configuration.GetSection("CookieOptions:Secure").Get<bool>(),
                        Domain = _configuration.GetSection("CookieOptions:Domain").Value
                    };
                    Response.Cookies.Append(_configuration.GetSection("CookieOptions:CookieName").Value, accessToken, cookieOptions);

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
        #region Private method
        private DateTime GetExpire()
        {
            DateTime? expires = null;
            string expireUnit = _configuration.GetSection("TokenSettings:ExpireUnit").Value;
            bool ret = int.TryParse(_configuration.GetSection("TokenSettings:Expire").Value, out int expireNumber);
            if (ret == false || string.IsNullOrEmpty(expireUnit))
            {
                expireUnit = "min";
                expireNumber = 30;
            }
            switch (expireUnit)
            {
                case "min":
                    expires = DateTime.Now.AddMinutes(expireNumber);
                    break;
                case "hour":
                    expires = DateTime.Now.AddHours(expireNumber);
                    break;
                case "day":
                    expires = DateTime.Now.AddDays(expireNumber);
                    break;
                case "month":
                    expires = DateTime.Now.AddMonths(expireNumber);
                    break;
            }
            return expires ?? DateTime.Now;
        }

        private SameSiteMode GetSameSite()
        {
            var sameSiteStr = _configuration.GetSection("CookieOptions:SameSite").Value;
            switch (sameSiteStr)
            {
                case "Unspecified":
                    return SameSiteMode.Unspecified;
                case "None":
                    return SameSiteMode.None;
                case "Lax":
                    return SameSiteMode.Lax;
                case "Strict":
                    return SameSiteMode.Strict;
                default:
                    return SameSiteMode.Unspecified;
            }
        }

        private async Task<bool> SetRefreshToken(RefreshToken newRefreshToken, ApplicationUser user)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = newRefreshToken.Expires,
                HttpOnly = _configuration.GetSection("CookieOptions:HttpOnly").Get<bool>(),
                SameSite = GetSameSite(),
                Secure = _configuration.GetSection("CookieOptions:Secure").Get<bool>(),
                Domain = _configuration.GetSection("CookieOptions:Domain").Value
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            Response.Cookies.Append("userName", user.UserName, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.RefreshTokenCreated = DateTimeExtensions.SetKindUtc(newRefreshToken.Created);
            user.RefreshTokenExpires = DateTimeExtensions.SetKindUtc(newRefreshToken.Expires);

            // Update refreshToken info to DB
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded == false)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Refresh token
        [HttpPost("refresh-token-customer")]
        public async Task<ActionResult<ApplicationUser>> RefreshTokenBasicCookie()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            string? userName = Request.Cookies["userName"];
            if (string.IsNullOrEmpty(userName)) return BadRequest("userName is invalid");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound(string.Format("Can not find user:{0}", userName));
            if (!user.RefreshToken.Equals(refreshToken))
            {
                return new UnauthorizedObjectResult("Invalid Refresh Token.");
            }
            else if (user.RefreshTokenExpires < DateTime.Now)
            {
                return new UnauthorizedObjectResult("Token expired.");
            }

            // Get group
            var customerGroup = new ApplicationGroup
            {
                Name = "Customer",
                ID = 2
            };
            var userGroup = _applicationUserGroupRepository.GetSingleByCondition(x => x.UserId == user.Id);
            //if (userGroup == null)
            //{
            //    return BadRequest("Not found user in user_group table");
            //}

            if (userGroup != null && userGroup.GroupId != 2) // if user not customer
            {
                //return BadRequest("Not found user");
                return new RedirectToRouteResult(new { page = "/Account/Error", area = "Identity", title = "Error", errormsg = "Not_found_user" });
            }

            string accessToken = _tokenManager.GenerateAccessToken(user, customerGroup);
            var newRefreshToken = _tokenManager.GenerateRefreshToken(user.UserName);
            bool ret = await SetRefreshToken(newRefreshToken, user);
            if (ret == false)
            {
                return BadRequest();
            }

            ApplicationUserVM returnUserInfo = new ApplicationUserVM
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
            };
            if (returnUserInfo != null)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = GetExpire(),
                    HttpOnly = _configuration.GetSection("CookieOptions:HttpOnly").Get<bool>(),
                    SameSite = GetSameSite(),
                    Secure = _configuration.GetSection("CookieOptions:Secure").Get<bool>(),
                    Domain = _configuration.GetSection("CookieOptions:Domain").Value
                };
                Response.Cookies.Append(_configuration.GetSection("CookieOptions:CookieName").Value, accessToken, cookieOptions);           
            }

            return new OkObjectResult(returnUserInfo);
        }
        #endregion
    }
}
