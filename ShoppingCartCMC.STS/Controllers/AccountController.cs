using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingCartCMC.Server.Shared.Common;
using ServiceStack;
using Microsoft.Extensions.Localization;
using System.Reflection;
using ShoppingCartCMC.Server.Shared.Identity;
using ShoppingCartCMC.STS.Models.AccountViewModels;
using ShoppingCartCMC.Server.Shared;
using ShoppingCartCMC.STS.Resources;

namespace ShoppingCartCMC.STS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IRedisEntityRepositoryHelper _redisHelper;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IPersistedGrantService _persistedGrantService;
        private readonly IConfiguration _config;
        private readonly IStringLocalizer _sharedLocalizer;

        //PW: dependency injection - parameters passed
        public AccountController(
            IRedisEntityRepositoryHelper redisHelper,
            UserManager<MyIdentityUser> userManager,
            IPersistedGrantService persistedGrantService,
            SignInManager<MyIdentityUser> signInManager,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IStringLocalizerFactory factory,
            IConfiguration config)
        {
            _redisHelper = redisHelper;
            _userManager = userManager;
            _persistedGrantService = persistedGrantService;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _httpContextAccessor = httpContextAccessor;
            _interaction = interaction;
            _clientStore = clientStore;
            _config = config;

            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _sharedLocalizer = factory.Create("SharedResource", assemblyName.Name);
        }


        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["IsInternalRequest"] = ServerUtility.IsInternalRequest(_config, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            var vm = await BuildLoginViewModelAsync(returnUrl, context);
            return View(vm);
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            var returnUrl = model.ReturnUrl;
            ViewData["IsInternalRequest"] = ServerUtility.IsInternalRequest(_config, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                bool twoFA_Enforced = _config.GetValue<bool>("AppSetting:TwoFA_Enforced");
                bool twoFA_Ignored = _config.GetValue<bool>("AppSetting:TwoFA_Ignored");

                //PW: (1) handle disabled user, and check AccountOpeningStatus
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    //PW: don't reveal user not found.
                    _logger.LogWarning("Invalid login attempt. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                    ModelState.AddModelError(string.Empty, _sharedLocalizer["INVALID_LOGIN_ATTEMPT"]);
                    return View(await BuildLoginViewModelAsync(model));
                }
                else if (!user.IsEnabled)
                {
                    bool passwordOk = await _userManager.CheckPasswordAsync(user, model.Password);

                    //PW: don't reveal user is disabled if password not matched.
                    if (passwordOk)
                    {
                        _logger.LogWarning("User is disabled. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        ModelState.AddModelError(string.Empty, _sharedLocalizer["USER_IS_DISABLED"]);
                        return View(await BuildLoginViewModelAsync(model));
                    }
                    else
                    {
                        _logger.LogWarning("Invalid login attempt. Disabled user, userEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        ModelState.AddModelError(string.Empty, _sharedLocalizer["INVALID_LOGIN_ATTEMPT"]);
                        return View(await BuildLoginViewModelAsync(model));
                    }
                }
                else
                {
                    bool passwordOk = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (!passwordOk)
                    {
                        //PW: don't reveal user not found.
                        _logger.LogWarning("Invalid login attempt. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        ModelState.AddModelError(string.Empty, _sharedLocalizer["INVALID_LOGIN_ATTEMPT"]);
                        return View(await BuildLoginViewModelAsync(model));
                    }
                    /*else*/
                    if (!user.EmailConfirmed)
                    {
                        _logger.LogWarning("Login denied as email is not verified. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        ModelState.AddModelError(string.Empty, _sharedLocalizer["EMAIL_NOT_VERIFIED"]);
                        return View(await BuildLoginViewModelAsync(model));
                    }
                }

                //PW: (2) check if 2FA is enforced in appsetting
                if (!twoFA_Ignored && twoFA_Enforced)
                {
                    if (!user.TwoFactorEnabled)
                    {
                        _logger.LogWarning("UserName/password matched but access blocked due to 2FA disabled, user must enable 2FA first. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        ModelState.AddModelError(string.Empty, _sharedLocalizer["2FA_DISABLED"]);
                        return View(await BuildLoginViewModelAsync(model));
                    }
                }

                //PW: (3) attempt sign-in.
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberLogin, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                    return RedirectToLocal(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    //PW: check if 2FA is ignored in configuration.
                    if (twoFA_Ignored)
                    {
                        await _signInManager.SignInAsync(user, model.RememberLogin);
                        _logger.LogInformation("User logged in. User 2FA is enabled but 2FA authentication is ignored/skipped. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        _logger.LogInformation("UserName/password matched, user is redirected to 2FA authentication. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        return RedirectToAction("LoginWith2fa", "Account", new { returnUrl = returnUrl, rememberMe = model.RememberLogin });
                    }
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                    return View("Lockout");
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt. UserEmail: {email}, IP: {ip}", model.Email, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                    ModelState.AddModelError(string.Empty, _sharedLocalizer["INVALID_LOGIN_ATTEMPT"]);
                    return View(await BuildLoginViewModelAsync(model));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(await BuildLoginViewModelAsync(model));
        }


        // GET: /Account/LoginWith2fa
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(string returnUrl = null, bool rememberMe = false)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            LoginWith2faViewModel vm = new LoginWith2faViewModel
            {
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            };

            return View(vm);
        }


        // POST: /Account/LoginWith2fa
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with '{UserName}' logged in with 2fa, IP: {ip}", user.UserName, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                return RedirectToLocal(model.ReturnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with '{UserName}' account locked out, IP: {ip}", user.UserName, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                return RedirectToAction("Lockout", "Account");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with '{UserName}', IP: {ip}", user.UserName, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                ModelState.AddModelError(string.Empty, _sharedLocalizer["INVALID_AUTHENTICATOR_CODE"]);
                return View(model);
            }
        }


        // POST: /Account/Cancel  
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Cancel(string returnUrl) //PW: add
        {
            string search = "redirect_uri=";
            string redirect_uri = null;

            if (returnUrl != null)
            {
                if (returnUrl.Contains(search))
                {
                    int startIndex = returnUrl.IndexOf(search);
                    int endIndex = returnUrl.IndexOf("&", startIndex);
                    redirect_uri = returnUrl.Substring(startIndex + search.Length, endIndex - startIndex - search.Length);

                    redirect_uri = HttpUtility.UrlDecode(redirect_uri);
                }
            }

            if (redirect_uri == null || redirect_uri == "")
                redirect_uri = "/home";

            return Redirect(redirect_uri);
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };

            return View(vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);


            if (User.Identity.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                var vm = new LogoutViewModel
                {
                    LogoutId = model.LogoutId
                };

                if (logout?.PostLogoutRedirectUri != null && logout?.PostLogoutRedirectUri != "")
                    return Redirect(logout?.PostLogoutRedirectUri);
                else
                    return View("LoggedOut", vm);
            }
            else
            {
                var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                var subjectId = HttpContext.User.Identity.GetSubjectId();

                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    if (model.LogoutId == null)
                    {
                        // if there's no current logout context, we need to create one
                        // this captures necessary info from the current logged in user
                        // before we signout and redirect away to the external IdP for signout
                        model.LogoutId = await _interaction.CreateLogoutContextAsync();
                    }

                    string url = "/Account/Logout?logoutId=" + model.LogoutId;
                    try
                    {
                        await _signInManager.SignOutAsync();
                        // await HttpContext.Authentication.SignOutAsync(idp, new AuthenticationProperties { RedirectUri = url });
                    }
                    catch (NotSupportedException)
                    {
                    }
                }

                // delete authentication cookie
                await _signInManager.SignOutAsync();

                string userName = HttpContext.User.Identity.Name;
                _logger.LogInformation("User with '{UserName}' logged out, IP: {ip}", userName, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());

                // set this so UI rendering sees an anonymous user
                HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

                var vm = new LoggedOutViewModel
                {
                    PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                    ClientName = logout?.ClientId,
                    SignOutIframeUrl = logout?.SignOutIFrameUrl
                };

                //PW: change
                if (logout?.PostLogoutRedirectUri != null && logout?.PostLogoutRedirectUri != "")
                    return Redirect(logout?.PostLogoutRedirectUri);
                else
                    return View("LoggedOut", vm);
            }
        }



        #region Helpers

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var loginProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var providers = loginProviders
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                });

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context?.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme));
                    }
                }
            }

            return new LoginViewModel
            {
                EnableLocalLogin = allowLocal, //PW: always true, we don't have external providers.
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
                ExternalProviders = providers.ToArray() //PW: array always empty, we don't have external providers.
            };
        }


        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.Email = model.Email;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }

}
