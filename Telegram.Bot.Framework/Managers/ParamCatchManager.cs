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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Managers
{
    /// <summary>
    /// 
    /// </summary>
    internal class ParamCatchManager : IParamManager
    {
        private bool _IsRead;
        private List<object> _Params = new List<object>();
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

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public string GetCommand()
        {
            throw new NotImplementedException();
        }

        public object[] GetParam()
        {
            return _Params.ToArray();
        }

        public bool IsReadParam()
        {
            return _IsRead;
        }

        public async Task<bool> ReadParam(TelegramContext context)
        {
            MessageEntity[] MessageEnityList = context.Update.Message?.Entities;
            // 如果已经开始读取参数了，那么就直接进入即可
            if (IsReadParam())
                return ReadParamIFOnlyOneCommand(MessageEnityList, context);

            // 什么都没有
            if (MessageEnityList.IsEmpty())
                return true;

            // 仅有指令
            if (MessageEnityList.Length == 1)
                return ReadParamIFOnlyOneCommand(MessageEnityList, context);

            // 指令后面跟着参数
            if (MessageEnityList.Length > 1)
                return ReadParamIFMultiCommand(MessageEnityList, context);
            return true;
        }

        private bool ReadParamIFOnlyOneCommand(MessageEntity[] MessageEnityList, TelegramContext context)
        {
            if (IsReadParam())
            {
                SetIsRead(false);
                return false;
            }
            else
            {
                SetIsRead(true);
                MessageEntity messageEntity = MessageEnityList.FirstOrDefault();
                if (messageEntity != null)
                {

                }
                return false;
            }
        }

        private bool ReadParamIFMultiCommand(MessageEntity[] MessageEnityList, TelegramContext context)
        {
            foreach (MessageEntity item in MessageEnityList)
            {
                
            }
            return false;
        }
    }
}
