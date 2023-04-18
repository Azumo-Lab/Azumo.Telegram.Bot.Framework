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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Groups;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.ExtensionMethods;
using Telegram.Bot.Types;
using static System.Collections.Specialized.BitVector32;

namespace Telegram.Bot.Framework.ChannelGroup
{
    /// <summary>
    /// 屏蔽群组垃圾信息
    /// </summary>
    public abstract class AbsGroupMessageProcess : IGroupMessageProcess
    {
        protected List<Regex> TargetRegexs { get; } = new List<Regex>();

        protected void AddRegex(string Regex)
        {
            TargetRegexs.Add(new Regex(Regex));
        }

        public virtual async Task Invoke(Message message, ITelegramSession Session)
        {
            try
            {
                string messageText = messageText = message.Text ?? string.Empty;
                foreach (Regex regex in TargetRegexs)
                {
                    if (regex.IsMatch(messageText))
                    {
                        await TargetMessageProcess(messageText, message, Session);
                    }
                }
                await OtherMessageProcess(messageText, message, Session);
            }
            catch (Exception)
            {
            }
        }

        protected virtual Task TargetMessageProcess(string messageText, Message message, ITelegramSession Session)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OtherMessageProcess(string messageText, Message message, ITelegramSession Session)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task DeleteMessage(Message message, ITelegramSession Session)
        {
            await Session.BotClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
        }
    }
}
