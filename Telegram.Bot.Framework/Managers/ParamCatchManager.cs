//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Managers
{
    /// <summary>
    /// 帮助获取创建参数
    /// </summary>
    internal class ParamCatchManager : IParamManager
    {
        private bool _IsRead;
        private bool _WaitForInput;
        private string _CommandName;
        private List<object> _Params = new List<object>();
        private List<ParamInfos> _ParamInfos = new List<ParamInfos>();
        private int _ErrorCount;//重试次数
        private const int _ErrorAllCount = 3;//重试机会
        #region Private方法群
        private void SetIsRead(bool flag)
        {
            _IsRead = flag;
        }
        private void SetParam(object obj)
        {
            _Params.Add(obj);
        }
        #endregion

        /// <summary>
        /// 取消，重置全部状态
        /// </summary>
        public void Cancel()
        {
            _IsRead = false;
            _WaitForInput = false;
            _CommandName = null;
            _ErrorCount = 0;

            _Params.Clear();
            _ParamInfos.Clear();
        }

        /// <summary>
        /// 获取当前参数列表的指令
        /// </summary>
        /// <returns>指令名称</returns>
        public string GetCommand()
        {
            return _CommandName;
        }

        /// <summary>
        /// 获取读取后的参数列表
        /// </summary>
        /// <returns>参数列表</returns>
        public object[] GetParam()
        {
            return _Params.ToArray();
        }

        /// <summary>
        /// 是否处于读取状态
        /// </summary>
        /// <returns>True:读取状态/False:非读取状态</returns>
        public bool IsReadParam()
        {
            return _IsRead;
        }

        /// <summary>
        /// 开始读取参数
        /// </summary>
        /// <param name="Context">Context</param>
        /// <returns>True:读取完成/False:需要继续读取</returns>
        public async Task<bool> ReadParam(TelegramContext Context)
        {
            // 如果已经开始读取参数了，那么就直接进入即可
            if (IsReadParam())
                return await ReadParamContinue(Context);

            // 取消掉先前的指令
            Cancel();

            MessageEntity[] MessageEnityList = Context.Update.Message?.Entities;

            // 什么都没有
            if (MessageEnityList.IsEmpty())
                return false;

            // 仅有指令
            if (MessageEnityList.Length == 1)
                return await ReadParamIFOnlyOneCommand(Context);

            // 指令后面跟着参数
            if (MessageEnityList.Length > 1)
                return await ReadParamIFMultiCommand(MessageEnityList, Context);
            return true;
        }

        /// <summary>
        /// 读取只有一个指令的参数，通过发送提示消息的形式来进行互动，指引用户填写参数
        /// </summary>
        /// <param name="MessageEnityList">内容列表</param>
        /// <param name="Context">Context</param>
        /// <returns></returns>
        private async Task<bool> ReadParamIFOnlyOneCommand(TelegramContext Context)
        {
            string CommandName = Context.GetCommand();

            // 发送的消息不是指令
            if (CommandName.IsEmpty())
                return false;

            // 获取指令信息
            IControllerManager controllerManager = Context.UserScope.GetService<IControllerManager>();
            CommandInfos CommandInfo = controllerManager.GetCommandInfo(CommandName);

            // 没有这个指令
            if (CommandInfo.IsNull())
                return false;

            _CommandName = CommandName;

            // 这条指令没有参数
            if (CommandInfo.ParamInfos.IsEmpty())
                return true;

            SetIsRead(true);
            _ParamInfos.AddRange(CommandInfo.ParamInfos);

            // 有参数，需要接收参数
            return await ReadParamContinue(Context);
        }

        /// <summary>
        /// 处于读取状态下，继续读取参数
        /// </summary>
        /// <param name="MessageEnityList">内容列表</param>
        /// <param name="Context">Context</param>
        /// <returns>True:读取完成/False:未完成继续读取</returns>
        private async Task<bool> ReadParamContinue(TelegramContext Context)
        {
            ParamInfos Param = _ParamInfos.FirstOrDefault();
            // 判断是否还有参数了
            if (Param.IsNull())
            {
                // 无参数，读取完成
                SetIsRead(false);
                return true;
            }

            if (_WaitForInput)
            {
                bool result;
                if (result = await GetParam(Context.UserScope, Param, Context))
                    _ParamInfos.Remove(Param);
                _WaitForInput = false;
                if (!result && _ErrorCount++ >= _ErrorAllCount)
                {
                    Cancel();
                    return false;
                }
                // 需要继续读取
                return await ReadParamContinue(Context);
            }
            else
            {
                await SendMessage(Context.UserScope, Param);
                _WaitForInput = true;
                return false;
            }
        }

        /// <summary>
        /// 发送用户提示消息
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <param name="paramInfos">参数信息</param>
        /// <returns>无</returns>
        private async Task SendMessage(IServiceProvider serviceProvider, ParamInfos paramInfos)
        {
            IParamMessage paramMessage = (IParamMessage)ActivatorUtilities.CreateInstance(serviceProvider, paramInfos.CustomMessageType, Array.Empty<object>());
            await paramMessage.SendMessage(paramInfos.MessageInfo);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <param name="paramInfos">参数信息</param>
        /// <param name="context">TelegramContext</param>
        /// <returns>True:参数获取成功/False:参数检查失败无法获取参数</returns>
        private async Task<bool> GetParam(IServiceProvider serviceProvider, ParamInfos paramInfos, TelegramContext context)
        {
            bool result;
            IParamMaker paramMaker = (IParamMaker)ActivatorUtilities.CreateInstance(serviceProvider, paramInfos.CustomParamMaker ?? paramInfos.ParamType, Array.Empty<object>());
            if (result = await paramMaker.ParamCheck(context, serviceProvider))
            {
                object paramVal = await paramMaker.GetParam(context, serviceProvider);
                SetParam(paramVal);
            }
            return result;
        }

        /// <summary>
        /// 读取（一个指令，后面带有参数）这种形式的指令
        /// </summary>
        /// <param name="MessageEnityList">内容列表</param>
        /// <param name="Context">Context</param>
        /// <returns>True:读取结束/False:继续读取</returns>
        private Task<bool> ReadParamIFMultiCommand(MessageEntity[] MessageEnityList, TelegramContext Context)
        {
            string CommandName;
            if ((CommandName = Context.GetCommand()).IsEmpty())
                return Task.FromResult(true);

            _CommandName = CommandName;

            IControllerManager controllerManager = Context.UserScope.GetService<IControllerManager>();
            CommandInfos commandInfos = controllerManager.GetCommandInfo(CommandName);
            if (commandInfos.IsNull())
                return Task.FromResult(true);

            _ParamInfos.AddRange(commandInfos.ParamInfos);

            return Task.FromResult(true);
        }
    }
}
