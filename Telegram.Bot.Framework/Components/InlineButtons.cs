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

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Components
{
    /// <summary>
    /// 返回消息的按钮
    /// </summary>
    public class InlineButtons
    {
        public string Text { get; private set; }

        public Action<TelegramContext> Callback { get; private set; }

        public string Url { get; set; }

        public LoginUrl LoginUrl { get; set; }

        public WebAppInfo WebApp { get; set; }

        public string SwitchInlineQuery { get; set; }

        public string SwitchInlineQueryCurrentChat { get; set; }

        public CallbackGame CallbackGame { get; set; }

        public bool Pay { get; set; }

        public static InlineButtons WithCallback(string text, Action<TelegramContext> callback) =>
            new() { Text = text, Callback = callback };

        public static InlineButtons WithUrl(string text, string url) =>
            new() { Text = text, Url = url };

        public static InlineButtons WithLoginUrl(string text, LoginUrl loginUrl) =>
            new() { Text = text, LoginUrl = loginUrl };

    }
}
