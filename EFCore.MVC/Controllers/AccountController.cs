using AutoMapper;
using EFCore.DTO;
using EFCore.MVC.Constants;
using EFCore.MVC.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EFCore.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserVM model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterAsync(_mapper.Map<RegisterUserDTO>(model));
                if (result.Succeeded)
                {
                    var callbackUrl = Url.Action(
                         nameof(RegisterConfirmationSuccess),
                         controller: "Account",
                         values: new { userId = result.Id, code = result.Code },
                         protocol: Request.Scheme);

                    var httpStatus = await _accountService.SendEmailAsync(new EmailDTO
                    {
                        Recipient = result.Email,
                        Subject = "EFCore MVC Registration",
                        HtmlContent = $"<p><strong>Hi { result.FirstName },</strong></p><p>Thanks so much for joining EFCore MVC! To finish signing up, you just need to confirm that we got your email right</p><br /><a style=\"background-color: orange; color: white; padding: 10px;\" href='{HtmlEncoder.Default.Encode(callbackUrl)}'>confirm my email</a><br/>"
                    });

                    if (result.RequireConfirmedEmail)
                    {
                        return RedirectToAction(nameof(RegisterConfirmation), new { email = model.Email });
                    }
                    else
                    {
                        await _accountService.SignInAsync(result.Id, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            return View(model);
        }

        public async Task<IActionResult> RegisterConfirmationSuccess(string userId, string code)
        {
            await _accountService.RegisterConfirmationSuccessAsync(userId, code);
            return View();
        }

        public IActionResult RegisterConfirmation(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn(string returnUrl = null)
        {
            LoginVM model = new LoginVM
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginVM input, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var isSuccess = await _accountService.PasswordSignInAsync(input.Email,
                    input.Password, input.RememberMe, lockoutOnFailure: true);

                if (isSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attrempt.");
                }

                #region MVC Specific Registration

                //var result = await _signInManager.PasswordSignInAsync(input.Email,
                //    input.Password, input.RememberMe, lockoutOnFailure: true);

                //if (result.Succeeded)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                //if (result.RequiresTwoFactor)
                //{

                //}
                //if (result.IsLockedOut)
                //{
                //    //UserAccount Lock...display Page
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attrempt.");
                //}

                #endregion
            }

            input.ReturnUrl = returnUrl;

            return View(input);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut(string returnUrl = null)
        {
            await _accountService.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToAction(nameof(LogIn));
            }
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("ExternalLogin")]
        public Task ExternalLogin(string id)
        {
            return HttpContext.ChallengeAsync(id, new AuthenticationProperties
            {
                //RedirectUri = Url.Action("UserInfo", "Account")
                RedirectUri = Url.Action("Index", "Home")
            });
        }

        [Route("userinfo")]
        [Authorize]
        public IActionResult UserInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignOutExternal()
        {
            await HttpContext.SignOutAsync(ApplicationAuthenticationSchemes.DefaultAuthenticationScheme);
            Response.Cookies.Delete(ApplicationAuthenticationSchemes.DefaultAuthenticationScheme);
            return RedirectToAction(nameof(LogIn));
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public IActionResult ExternalLogin_ToCheck(string provider, string returnUrl = null)
        //{
        //    // Request a redirect to the external login provider.
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return new ChallengeResult(provider, properties);
        //}

        //[AllowAnonymous]
        //public async Task<IActionResult> ExternalLoginCallback_ToCheck(string returnUrl = null, string remoteError = null)
        //{
        //    returnUrl = returnUrl ?? Url.Content("~/");

        //    LoginVM model = new LoginVM
        //    {
        //        ReturnUrl = returnUrl,
        //        ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        //    };

        //    if (remoteError != null)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error from external provider : { remoteError }");
        //        return View("Login", model);
        //    }

        //    /*This should be used to register external login in Database, but this always return null value, might
        //     need to check facebook developer settings. It is suspected will work once this site is deployed in www.*/
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Error loading external login information");
        //        return View("Login", model);
        //    }

        //    var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
        //        info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        //    if (signInResult.Succeeded)
        //    {
        //        return LocalRedirect(returnUrl);
        //    }
        //    else
        //    {
        //        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //        if (email != null)
        //        {
        //            var user = await _userManager.FindByEmailAsync(email);
        //            if (user == null)
        //            {
        //                user = new ApplicationUser
        //                {
        //                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
        //                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
        //                };

        //                await _userManager.CreateAsync(user);
        //            }

        //            await _userManager.AddLoginAsync(user, info);
        //            await _signInManager.SignInAsync(user, isPersistent: false);

        //            return LocalRedirect(returnUrl);
        //        }

        //        ViewBag.ErrorTitle = $"Email claim not receieved from : { info.LoginProvider}";
        //        ViewBag.ErrorMessage = "Please contact support";

        //        return View("Error");
        //    }
        //}

        //public async Task<IActionResult> LoginExternal(string returnURL)
        //{
        //    LoginVM model = new LoginVM
        //    { 
        //        ReturnUrl = returnURL,
        //        ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        //    };

        //    return View(model);
        //}

        [Authorize]
        public async Task<IActionResult> UpdateUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var applicationUserDTO = await _accountService.GetUser(id);

            return View(_mapper.Map<ApplicationUserVM>(applicationUserDTO));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdateUser(ApplicationUserVM model)
        {
            if (ModelState.IsValid)
            {
                var applicationUserDto = await _accountService.UpdateUserAsync(_mapper.Map<ApplicationUserDTO>(model));
                return RedirectToAction(nameof(Success), _mapper.Map<ApplicationUserVM>(applicationUserDto));
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeUserPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.CurrentPassword.Equals(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Can not reuse password. Please try again.");
                }
                else
                {
                    var result = await _accountService.ChangePasswordAsync(model.Id, model.CurrentPassword, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ChangePasswordSuccess));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ForgotPasswordAsync(model.Email);
                if (result.Succeeded == false)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ModelState.AddModelError(string.Empty, "Unable to process password reset.");
                    return View(model);
                }

                var callbackUrl = Url.Action(
                         nameof(ResetPassword),
                         controller: "Account",
                         values: new { userId = result.Id, code = result.Code },
                         protocol: Request.Scheme);

                var httpStatus = await _accountService.SendEmailAsync(new EmailDTO
                {
                    Recipient = result.Email,
                    Subject = "EFCore MVC Password Reset",
                    HtmlContent = $"<p><strong>Hi { result.FirstName },</strong></p><p>Please reset your password by clicking here</p><br /><a style=\"background-color: orange; color: white; padding: 10px;\" href='{HtmlEncoder.Default.Encode(callbackUrl)}'>reset my password</a><br/>"
                });

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return RedirectToAction(nameof(LogIn));
            }

            return View(new ResetPasswordVM
            {
                Id = userId,
                Code = code
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ResetPasswordAsync(model.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ResetPasswordSuccess));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            return View(model);
        }

        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        public IActionResult ChangePasswordSuccess()
        {
            ViewBag.Message = "Password successfully updated.";
            return View();
        }

        public IActionResult Success(ApplicationUserVM model)
        {
            return View(model);
        }
    }
}