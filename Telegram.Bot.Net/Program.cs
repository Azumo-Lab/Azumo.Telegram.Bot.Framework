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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.FrameworkHelper;

namespace Telegram.Bot.Net
{
    class Program : IConfig
    {
        static void Main(string[] args)
        {
            var Secrets = new ConfigurationBuilder().AddUserSecrets("98def42c-77dc-41cb-abf6-2c402535f4cb").Build();

            string Token = Secrets.GetSection("Token").Value;
            string Proxy = Secrets.GetSection("Proxy").Value;
            int Port = int.Parse(Secrets.GetSection("Port").Value);

            var bot = TelegramBotManger.CreateConfig()
                .SetToken(Token)
                .SetProxy(Proxy, Port)
                .SetConfig(new Program())
                .Build();

            bot.Start();
            
            //TestHelper.Test();

        }

        public void Config(IServiceCollection telegramServices)
        {
            //throw new NotImplementedException();
        }
    }
}
