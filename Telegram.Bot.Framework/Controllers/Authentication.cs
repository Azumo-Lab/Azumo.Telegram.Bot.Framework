using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IControllerFilter))]
    internal class Authentication : IControllerFilter
    {
        /// <summary>
        /// 开始执行权限认证
        /// </summary>
        /// <param name="tGChat"></param>
        /// <param name="botCommand"></param>
        /// <returns></returns>
        public virtual async Task<bool> Execute(TGChat tGChat, BotCommand botCommand)
        {
            if (botCommand?.AuthenticateAttribute == null)
                return true;

            AuthenticationCode authenticationCode;
            IAuthenticate authenticate = tGChat.Authenticate;
            if (authenticate == null || authenticate.RoleName.Count == 0)
            {
                authenticationCode = AuthenticationCode.NeedToLogIn;
            }
            else
            {
                AuthenticateAttribute authenticateAttribute = botCommand.AuthenticateAttribute;
                bool flag = await tGChat.Authenticate.IsAuthenticated(tGChat, authenticateAttribute);
                authenticationCode = flag ? AuthenticationCode.Success : AuthenticationCode.PermissionDenied;
            }

            // 后续动作（不等待执行完成）
            IAuthenticationAction authenticationAction = tGChat.UserService.GetService<IAuthenticationAction>();
            _ = (authenticationAction?.InvokeAction(authenticationCode, tGChat, botCommand).ConfigureAwait(false));

            // 返回结果（不等待上述动作执行完成）
            return authenticationCode != AuthenticationCode.Success;
        }
    }
}
