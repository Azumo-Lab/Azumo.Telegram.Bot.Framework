using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Telegram.Bot.Framework.FrameworkHelper;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal class DelegateManger : IDelegateManger
    {
        private readonly Dictionary<string, MethodInfo> Command_MethodMap;
        private readonly Dictionary<string, Delegate> Command_DelegateMap = new Dictionary<string, Delegate>();

        internal DelegateManger(Dictionary<string, MethodInfo> Command_MethodMap)
        {
            this.Command_MethodMap = Command_MethodMap;
        }

        public Delegate CreateDelegate(string Command, object controller)
        {
            if (!Command_DelegateMap.ContainsKey(Command))
            {
                Delegate action = DelegateHelper.CreateDelegate(Command_MethodMap[Command], controller);
                Command_DelegateMap.Add(Command, action);
            }
            return Command_DelegateMap[Command];
        }
    }
}
