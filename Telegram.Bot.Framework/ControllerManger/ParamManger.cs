using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal class ParamManger : IParamManger
    {
        private static Dictionary<long, List<object>> Params = new Dictionary<long, List<object>>();
        private static Dictionary<long, bool> ParamsOK = new Dictionary<long, bool>();
        private static Dictionary<long, string> CHatID_Command = new Dictionary<long, string>();
        private static Dictionary<string, ParameterInfo[]> Command_ParamInfo = new Dictionary<string, ParameterInfo[]>();

        public ParamManger() { }

        public ParamManger(Dictionary<string, ParameterInfo[]> Command_ParamInfo)
        {
            ParamManger.Command_ParamInfo = Command_ParamInfo;
        }

        public void Cancel(TelegramContext context)
        {
            Params.Remove(context.ChatID);
            ParamsOK.Remove(context.ChatID);
            CHatID_Command.Remove(context.ChatID);
        }

        public object[] GetParam(TelegramContext context)
        {
            var ID = context.ChatID;
            if (ParamsOK.ContainsKey(ID))
                if (ParamsOK[ID] && Params.ContainsKey(ID))
                    return Params[context.ChatID].ToArray();
            return null;
        }

        public bool IsReadParam(TelegramContext context)
        {
            long id = context.ChatID;
            return ParamsOK.ContainsKey(id) && ParamsOK[id];
        }

        public void ReadParam(TelegramContext context, IServiceProvider serviceProvider)
        {
            var chatID = context.ChatID;
            string Command = CHatID_Command[chatID];
            var parainfos = Command_ParamInfo[Command];

            if (!Params.ContainsKey(chatID))
            {
                Params.Add(chatID, new List<object>());
                ParamsOK.Add(chatID, false);
            }


        }

        public void SetCommand(string Command, TelegramContext context)
        {
            if (CHatID_Command.ContainsKey(context.ChatID))
            {
                CHatID_Command.Remove(context.ChatID);
            }
            CHatID_Command.Add(context.ChatID, Command);
        }
    }
}
