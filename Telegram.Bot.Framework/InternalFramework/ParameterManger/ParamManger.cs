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
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;

namespace Telegram.Bot.Framework.InternalFramework.ParameterManger
{
    internal class ParamManger : IParamManger
    {
        private static Dictionary<long, List<object>> Params = new Dictionary<long, List<object>>();
        private static Dictionary<long, (bool OK, bool Reading)> ParamsOK = new Dictionary<long, (bool OK, bool Reading)>();
        private static Dictionary<long, string> CHatID_Command = new Dictionary<long, string>();
        private static Dictionary<string, List<(Type ParamType, string ParamMessage)>> Command_ParamInfo = new Dictionary<string, List<(Type ParamType, string ParamMessage)>>();
        private static Dictionary<long, int> User_Index = new Dictionary<long, int>();

        private readonly IServiceProvider service;
        private readonly TelegramContext context;

        public ParamManger() { }

        public static void SetDic(Dictionary<string, List<(Type ParamType, string ParamMessage)>> Command_ParamInfo)
        {
            ParamManger.Command_ParamInfo = Command_ParamInfo;
        }

        public ParamManger(IServiceProvider serviceProvider)
        {
            service = serviceProvider;
            context = service.GetService<TelegramContext>();
        }

        public void Cancel()
        {
            Params.Remove(context.ChatID);
            ParamsOK.Remove(context.ChatID);
            CHatID_Command.Remove(context.ChatID);
        }

        public object[] GetParam()
        {
            var ID = context.ChatID;
            if (ParamsOK.ContainsKey(ID))
                if (ParamsOK[ID].OK && Params.ContainsKey(ID))
                    return Params[context.ChatID].ToArray();
            return null;
        }

        public bool IsReadParam()
        {
            long id = context.ChatID;
            return ParamsOK.ContainsKey(id) && !ParamsOK[id].OK;
        }

        public void SetCommand()
        {
            string Command;
            if ((Command = context.GetCommand()) == null)
                return;

            if (CHatID_Command.ContainsKey(context.ChatID))
            {
                CHatID_Command.Remove(context.ChatID);
            }
            CHatID_Command.Add(context.ChatID, Command);
        }

        public async Task<bool> StartReadParam()
        {
            var command = CHatID_Command[context.ChatID];
            var ParamInfos = Command_ParamInfo[command];
            if (ParamInfos == null)
                return true;

            if (!ParamsOK.ContainsKey(context.ChatID))
                ParamsOK.Add(context.ChatID, (false, false));

            if (ParamsOK[context.ChatID].Reading == true)
            {
                IParamMaker maker = (IParamMaker)service.GetService(ParamInfos[User_Index[context.ChatID]].ParamType);

                Task<object> result = maker.GetParam(context, service);

                if (!Params.ContainsKey(context.ChatID))
                    Params.Add(context.ChatID, new List<object>());
                Params[context.ChatID].Add(result.Result);

                ParamsOK[context.ChatID] = (ParamsOK[context.ChatID].OK, false);

                User_Index[context.ChatID] += 1;
                if (User_Index[context.ChatID] < ParamInfos.Count)
                {
                    return false;
                }
                ParamsOK[context.ChatID] = (true, false);
                return true;
            }
            else
            {
                User_Index.Add(context.ChatID, 0);
                var info = ParamInfos[User_Index[context.ChatID]];

                IParamMessage message = service.GetService<IParamMessage>();
                await message.SendMessage(info.ParamMessage);
                ParamsOK[context.ChatID] = (ParamsOK[context.ChatID].OK, true);
                return false;
            }
            
        }

        public string GetCommand()
        {
            return CHatID_Command[context.ChatID];
        }
    }
}
