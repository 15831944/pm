using IdentityServer4.Validation;
using NanoFabric.IdentityServer.Interfaces.Services;
using NanoFabric.IdentityServer.Utilities;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Services
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
