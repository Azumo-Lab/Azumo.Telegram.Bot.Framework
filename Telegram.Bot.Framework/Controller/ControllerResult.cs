using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 控制器的执行结果
    /// </summary>
    public enum ControllerResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// 未认证
        /// </summary>
        Unauthorized,

        /// <summary>
        /// 无权限
        /// </summary>
        Forbidden,

        /// <summary>
        /// 等待参数
        /// </summary>
        WaitParamter,
    }
}
