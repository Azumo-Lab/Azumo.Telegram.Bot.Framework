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

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract.Bots;

namespace Telegram.Bot.SendMessageBot
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        static void Main(string[] _)
        {
            IConfigurationRoot Secrets = new ConfigurationBuilder().AddUserSecrets("338dec28-4b6e-4c66-9bc9-dfcd091de7fc").Build();

            string Token = Secrets.GetSection("Token").Value;
            string Proxy = Secrets.GetSection("Proxy").Value;
            int Port = int.Parse(Secrets.GetSection("Port").Value);

            ITelegramBot bot = TelegramBotManger.Create()
                .SetToken(Token)
                .SetProxy(Proxy, Port)
                .AddConfig<BotConfig>()
                .SetBotName("SendMessageBot")
                .Build();

            Task botTask = bot.BotStart();

            botTask.Wait();
        }
    }
}
