using IdentityServer4.Validation;
using LeadChina.PM.IdentityServer.Interfaces.Services;
using LeadChina.PM.IdentityServer.Utilities;
using System.Threading.Tasks;

namespace LeadChina.PM.IdentityServer.Services
{
    /// <summary>
    /// 密码验证
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public IUserService UserService { get; private set; }

        public ResourceOwnerPasswordValidator(IUserService userService)
        {
            UserService = userService;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await UserService.GetAsync(context.UserName, context.Password);

            if (user != null)
            {
                var claims = ClaimsUtility.GetClaims(user);
                context.Result = new GrantValidationResult(user.Id.ToString(), "password", claims);
            }
        }
    }
}
