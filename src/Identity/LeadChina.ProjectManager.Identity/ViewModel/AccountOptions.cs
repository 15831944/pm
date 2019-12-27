using Microsoft.AspNetCore.Server.IISIntegration;
using System;

namespace LeadChina.ProjectManager.Identity.ViewModel
{
    /// <summary>
    /// 账号配置
    /// </summary>
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;

        /// <summary>
        /// 有效时间
        /// </summary>
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;

        /// <summary>
        /// 指定正在使用的Windows身份验证方案
        /// </summary>
        public static readonly string WindowsAuthenticationSchemeName = IISDefaults.AuthenticationScheme;

        /// <summary>
        /// 如果用户使用windows认证，我们是否从windows加载组权限
        /// </summary>
        public static bool IncludeWindowsGroups = false;

        public static string InvalidCredentialsErrorMessage = "无效的用户名或密码";
    }
}
