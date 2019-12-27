using System.ComponentModel.DataAnnotations;

namespace LeadChina.ProjectManager.Identity.ViewModel
{
    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class LoginInputModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 记住登录
        /// </summary>
        public bool RememberLogin { get; set; }

        /// <summary>
        /// 跳转的链接
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
