//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
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
using Telegram.Bot.Framework.InternalFramework.Models;

namespace Telegram.Bot.Framework.InternalFramework.ParameterManager
{
    internal class ParamManager : IParamManager
    {
        private readonly Dictionary<string, CommandInfos> CommandCommandInfoMap;
        private readonly IServiceProvider UserServiceProvider;
        private readonly ITypeManager typeManger;

        private string CommandName;
        private List<ParamInfos> ParamInfo;
        private List<object> ParamList;
        private bool Reading = false;

        public ParamManager(IServiceProvider UserServiceProvider)
        {
            this.UserServiceProvider = UserServiceProvider;
            typeManger = this.UserServiceProvider.GetService<ITypeManager>();

            CommandCommandInfoMap = typeManger.GetCommandInfosDic();
        }

        public void Cancel()
        {
            CommandName = null;
            ParamInfo = null;
            ParamList = null;
        }

        private bool ReadOK()
        {
            ParamInfo = null;
            return true;
        }

        public string GetCommand()
        {
            return CommandName;
        }

        public object[] GetParam()
        {
            if (ParamList == null)
                return null;
            return ParamList.ToArray();
        }

        public bool IsReadParam()
        {
            return ParamInfo != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="OneTimeServiceProvider"></param>
        /// <returns>true 参数读取完毕/false 继续读取参数</returns>
        public async Task<bool> ReadParam(TelegramContext context, IServiceProvider OneTimeServiceProvider)
        {
            if (ParamInfo == null)
            {
                Cancel();
                if (string.IsNullOrEmpty(CommandName))
                {
                    CommandName = context.GetCommand();
                    if (string.IsNullOrEmpty(CommandName))
                        return ReadOK();
                }

                if (CommandCommandInfoMap.TryGetValue(CommandName, out CommandInfos cmdInfo))
                    ParamInfo = cmdInfo.ParamInfos.ToList();
                else
                    return ReadOK();
            }

            ReadParam:

            ParamInfos paramOne = ParamInfo.FirstOrDefault();

            if (!Reading)
            {
                if (paramOne == null)
                    return ReadOK();

                if (paramOne.MessageInfo != null)
                {
                    IParamMessage paramMessage = OneTimeServiceProvider.GetService<IParamMessage>();
                    await paramMessage.SendMessage(paramOne.MessageInfo);
                    Reading = true;
                    ParamList ??= new List<object>();
                    return false;
                }
            }
            else
            {
                IParamMaker paramMaker = (IParamMaker)OneTimeServiceProvider.GetService(paramOne.MessageType);
                ParamList.Add(await paramMaker.GetParam(context, OneTimeServiceProvider));
                ParamInfo.Remove(paramOne);
                Reading = false;
                if (!ParamInfo.Any())
                {
                    return ReadOK();
                }
                goto ReadParam;
            }

            return ReadOK();
        }
    }
}
