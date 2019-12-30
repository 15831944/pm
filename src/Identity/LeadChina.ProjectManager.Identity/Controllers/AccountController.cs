using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using LeadChina.ProjectManager.Identity.Filter;
using LeadChina.ProjectManager.Identity.Helper;
using LeadChina.ProjectManager.Identity.ViewModel;
using LeadChina.ProjectManager.SysSetting.BusiProcess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NanoFabric.IdentityServer.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeadChina.ProjectManager.Identity.Controllers
{
    /// <summary>
    /// 此示例控制器为本地和外部帐户实现了典型的登录/注销/设置流程
    /// 登录服务封装了与用户数据存储的交互，此数据存储仅在内存中，不能用于生产！
    /// 交互服务为UI提供了一种与identityserver通信的方式，用于验证和上下文检索
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserService _users;
        //private IIdentityModelService identityService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IAccountService _accountService;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserService users,
            IAccountService accountService)
        {
            // 如果TestUserStore不在DI容器中，我们将使用全局用户集
            // 在这里您可以插入自己的自定义身份管理库（例如：ASP.NET Identity）
            //_users = users;
            //identityService = _identityService;
            _users = users;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _accountService = accountService;
        }

        /// <summary>
        /// 登录流程的入口点
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // 构建用户视图模型
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // 我们只有一个登录选项，它供外部用户使用
                return RedirectToAction("Challenge", "External", new { provider = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// 处理从用户名/密码登录进来的请求
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // 检查我们是否处于授权请求的上下文中
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // 用户点击了“取消”按钮
            if (button != "login")
            {
                if (context != null)
                {
                    // 如果用户取消，则将结果发送回IdentityServer，就和IdentityServer拒绝同意一样（即使此客户端不需要同意）。
                    // 这将向客户端发送拒绝访问的OIDC错误响应。
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // 我们可以信任model.ReturnUrl，因为GetAuthorizationContextAsync返回了非空值
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // 如果客户机是PKCE，那么我们假设它是本机的，所以如何返回响应的这一更改是为了更好地为最终用户提供UX。
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // 因为我们没有有效的上下文，所以我们返回主页
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var user = _accountService.GetAcount(model.Username, model.Password);

                //if (await _users.ValidateCredentialsAsync(model.Username, model.Password))
                if (user != null)
                {
                    //var user = await _users.GetAsync(model.Username);
                    //await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.Id.ToString(), user.Username));
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.AccountNo, user.Id.ToString(), user.AccountName));

                    // 仅当用户选择“记住我”时才在此处设置显式过期，否则，我们依赖于cookie中间件中配置的过期时间。
                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    // 使用用户Id和用户名称发出验证cookie
                    //await HttpContext.SignInAsync(user.Id.ToString(), user.Username, props);
                    await HttpContext.SignInAsync(user.Id.ToString(), user.AccountName, props);

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // 如果客户机是PKCE，那么我们假设它是本机的，所以如何返回响应的这一更改是为了更好地为终端用户提供UX。
                            return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }
                        // 我们可以信任model.ReturnUrl，因为GetAuthorizationContextAsync返回了非空值
                        return Redirect(model.ReturnUrl);
                    }

                    // 请求本地页
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // 用户可能单击了恶意链接-应该记录
                        throw new Exception("无效链接");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "无效权限"));
                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
            }

            // 出现问题，显示有错误的表单
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }


        /// <summary>
        /// 显示注销页
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // 建立一个注销页面视图模型
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // 如果从IdentityServer正确地验证了注销请求，那么我们不需要显示提示，只需直接将用户注销即可。
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// 注销
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // 建立一个注销页面视图模型
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // 删除本地身份验证cookie
                await HttpContext.SignOutAsync();

                // 引发注销事件
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // 检查是否需要在上游 identity 中触发注销
            if (vm.TriggerExternalSignout)
            {
                // 建立一个返回URL，以便上游提供者在用户注销后重定向回我们。这样我们就可以完成单点注销处理。
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });
                // 这将触发重定向到外部提供程序以进行注销
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        /// <summary>
        /// 构建登录视图模型
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                // 这是为了使用户界面短路，并且只触发一个外部IdP
                return new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                    ExternalProviders = new ExternalProvider[] { new ExternalProvider { AuthenticationScheme = context.IdP } }
                };
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes.Where(x => x.DisplayName != null ||
                x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase)
            ).Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName,
                AuthenticationScheme = x.Name
            }
            ).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}
