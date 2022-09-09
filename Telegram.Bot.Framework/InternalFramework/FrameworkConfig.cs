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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Telegram.Bot.Framework.InternalFramework.Mangers;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.InternalFramework.ParameterManger;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Framework.TelegramException;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Framework.InternalFramework
{
    internal class FrameworkConfig : IConfig
    {
        private readonly IServiceProvider serviceProvider;
        public FrameworkConfig(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 框架相关的一些设置
        /// </summary>
        /// <param name="telegramServices"></param>
        public void Config(IServiceCollection telegramServices)
        {
            BotInfos botInfos = serviceProvider.GetService<BotInfos>();
            HttpClient httpClient = serviceProvider.GetService<HttpClient>();

            telegramServices.AddScoped<IParamMessage, StringParamMessage>();
            telegramServices.AddScoped<TelegramUser>();
            telegramServices.AddScoped(x =>
            {
                return new TelegramContext();
            });

            telegramServices.AddTransient<ITelegramUserScopeManger, TelegramUserScopeManger>();
            telegramServices.AddTransient<ITelegramUserScope, TelegramUserScope>();

            telegramServices.AddSingleton(new CancellationTokenSource());
            telegramServices.AddSingleton<IUpdateHandler, UpdateHandler>();
            telegramServices.AddSingleton<ITypeManger>(new TypeManger(telegramServices));
            telegramServices.AddSingleton<IBotNameManger>(x => 
            {
                BotNameManger botNameManger = new(x)
                {
                    BotName = botInfos.BotName
                };
                return botNameManger;
            });
            telegramServices.AddSingleton<ITelegramBotClient>(x => 
            {
                if (httpClient == null)
                    return new TelegramBotClient(botInfos.Token);
                else
                    return new TelegramBotClient(botInfos.Token, httpClient);
            });

            telegramServices.AddControllers();
        }
    }

    internal static class ServiceCollectionEx
    {
        /// <summary>
        /// 添加控制器
        /// </summary>
        /// <param name="services"></param>
        public static void AddControllers(this IServiceCollection services)
        {
            //添加进入services
            services.AddScoped<IControllersManger, ControllersManger>();
            services.AddScoped<IDelegateManger, ControllersManger>();
            services.AddScoped<IParamManger, ParamManger>();
        }

        /// <summary>
        /// 添加认证
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthentication(this IServiceCollection services)
        {

        }
    }
}
