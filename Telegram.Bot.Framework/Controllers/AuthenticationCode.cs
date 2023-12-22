using System.Diagnostics;

namespace Telegram.Bot.Framework.Controllers
{
    /// <summary>
    /// 权限认证结果
    /// </summary>
    public enum AuthenticationCode
    {
        /// <summary>
        /// 需要登陆
        /// </summary>
        [DebuggerDisplay("需要登录")] NeedToLogIn,

        /// <summary>
        /// 没有权限
        /// </summary>
        [DebuggerDisplay("权限不足")] PermissionDenied,

        /// <summary>
        /// 成功
        /// </summary>
        [DebuggerDisplay("成功")] Success,
    }
}
