using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal class ControllersManger : IControllersManger
    {
        private readonly Dictionary<string, Type> Command_ControllerMap;

        internal ControllersManger(Dictionary<string, Type> Command_ControllerMap)
        {
            this.Command_ControllerMap = Command_ControllerMap;
        }

        public object GetController(string CommandName, IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(Command_ControllerMap[CommandName]);
        }
    }
}
