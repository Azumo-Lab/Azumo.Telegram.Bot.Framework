using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Controllers
{
    /// <summary>
    /// 认证动作
    /// </summary>
    public interface IAuthenticationAction
    {
        /// <summary>
        /// 根据认证返回结果执行动作
        /// </summary>
        /// <remarks>
        /// 程序传入结果 <see cref="AuthenticationCode"/> 判断执行动作
        /// </remarks>
        /// <param name="authenticationCode"></param>
        /// <param name="tGChat"></param>
        /// <param name="botCommand"></param>
        /// <returns></returns>
        public Task InvokeAction(AuthenticationCode authenticationCode, TelegramUserChatContext tGChat, BotCommand botCommand);
    }
}
