using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal interface IParamManger
    {
        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceProvider"></param>
        void ReadParam(TelegramContext context, IServiceProvider serviceProvider);

        /// <summary>
        /// 是否处于读取参数的模式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool IsReadParam(TelegramContext context);

        /// <summary>
        /// 取消读取参数
        /// </summary>
        /// <param name="context"></param>
        void Cancel(TelegramContext context);

        /// <summary>
        /// 设置指令
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="context"></param>
        void SetCommand(string Command, TelegramContext context);

        /// <summary>
        /// 获取读取过后的参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object[] GetParam(TelegramContext context);
    }
}
