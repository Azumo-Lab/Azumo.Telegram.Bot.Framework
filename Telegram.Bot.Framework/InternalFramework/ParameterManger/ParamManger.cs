//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Net/>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.InternalFramework.ParameterManger
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
            return ParamsOK.ContainsKey(id) && !ParamsOK[id];
        }

        public void SetCommand(string Command, TelegramContext context)
        {
            if (CHatID_Command.ContainsKey(context.ChatID))
            {
                CHatID_Command.Remove(context.ChatID);
            }
            CHatID_Command.Add(context.ChatID, Command);
        }

        public bool StartReadParam(TelegramContext context, IServiceProvider serviceProvider)
        {
            var chatID = context.ChatID;
            string Command = CHatID_Command[chatID];

            var parainfos = Command_ParamInfo[Command];

            if (!Params.ContainsKey(chatID))
            {
                Params.Add(chatID, new List<object>());
                ParamsOK.Add(chatID, false);
            }

            IParamMessage paramMessage = serviceProvider.GetService<IParamMessage>();
            paramMessage.SendMessage("", context);

            return false;
        }
    }
}
