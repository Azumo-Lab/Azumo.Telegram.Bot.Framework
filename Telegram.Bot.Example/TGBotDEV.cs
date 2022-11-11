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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Net
{
    /// <summary>
    /// 开发测试机器人
    /// </summary>
    public class TGBotDEV : IConfig
    {
        static void Main(string[] _)
        {
            IConfigurationRoot Secrets = new ConfigurationBuilder().AddUserSecrets("98def42c-77dc-41cb-abf6-2c402535f4cb").Build();

            string Token = Secrets.GetSection("Token").Value;
            string Proxy = Secrets.GetSection("Proxy").Value;
            int Port = int.Parse(Secrets.GetSection("Port").Value);

            ITelegramBot bot = TelegramBotManger.Create()
                .SetToken(Token)
                .SetProxy(Proxy, Port)
                .AddConfig<TGBotDEV>()
                .SetBotName("DEV1")
                .Build();

            Task botTask = bot.BotStart();

            botTask.Wait();
        }

        public void ConfigureServices(IServiceCollection telegramServices)
        {
            telegramServices.AddUserAuthentication();
        }
    }
}
