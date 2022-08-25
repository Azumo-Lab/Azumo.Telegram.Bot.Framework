using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal class ControllersManger : IControllersManger
    {
        private readonly Dictionary<string, Type> Command_ControllerMap;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Command_ControllerMap"></param>
        internal ControllersManger(Dictionary<string, Type> Command_ControllerMap)
        {
            this.Command_ControllerMap = Command_ControllerMap;
        }

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <param name="CommandName"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object GetController(string CommandName, IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(Command_ControllerMap[CommandName]);
        }

        /// <summary>
        /// 检查是否有支持该指令的控制器
        /// </summary>
        /// <param name="CommandName">要测试的指令</param>
        /// <returns></returns>
        public bool HasCommand(string CommandName)
        {
            return Command_ControllerMap.ContainsKey(CommandName);
        }
    }
}
