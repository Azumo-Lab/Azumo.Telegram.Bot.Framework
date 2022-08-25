using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal class ParamManger : IParamManger
    {
        private static Dictionary<long, List<object>> Params = new Dictionary<long, List<object>>();
        private static Dictionary<long, bool> ParamsOK = new Dictionary<long, bool>();

        public object[] CreateParams(string Command, TelegramContext context)
        {
            throw new NotImplementedException();
        }

        public bool IsReadParam(TelegramContext context)
        {
            long id = context.ChatID;
            return ParamsOK.ContainsKey(id) && ParamsOK[id];
        }
    }
}
