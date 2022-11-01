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
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Authentications;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.Managers;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.InternalFramework.ParameterManager;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Framework.TelegramException;
using Telegram.Bot.Framework.UpdateTypeActions;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Framework.InternalFramework
{
    /// <summary>
    /// 整个框架的一些相关配置
    /// </summary>
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
        public void ConfigureServices(IServiceCollection telegramServices)
        {
            BotInfos botInfos = serviceProvider.GetService<BotInfos>();
            HttpClient httpClient = serviceProvider.GetService<HttpClient>();

            telegramServices.AddScoped<IParamMessage, StringParamMessage>();
            telegramServices.AddScoped<TelegramUser>();
            telegramServices.AddScoped<ICallBackManager, CallBackManager>();
            telegramServices.AddScoped(x =>
            {
                return new TelegramContext();
            });

            telegramServices.AddSingleton<ITelegramUserScopeManager, TelegramUserScopeManager>();
            telegramServices.AddTransient<ITelegramUserScope, TelegramUserScope>();

            telegramServices.AddSingleton(new CancellationTokenSource());
            telegramServices.AddSingleton<IUpdateHandler, UpdateHandler>();
            telegramServices.AddSingleton<ITypeManager>(new TypeManager(telegramServices));
            telegramServices.AddSingleton<IBotNameManager>(x => 
            {
                BotNameManager botNameManger = new(x)
                {
                    BotName = botInfos.BotName
                };
                return botNameManger;
            });
            telegramServices.AddSingleton<ITelegramBotClient>(x => 
            {
                return httpClient == null ? new TelegramBotClient(botInfos.Token) : new TelegramBotClient(botInfos.Token, httpClient);
            });

            telegramServices.AddControllers();
            telegramServices.AddServiceTypes<AbstractActionInvoker>(ServiceLifetime.Singleton);
        }
    }

    public static class ServiceCollectionEx
    {
        /// <summary>
        /// 添加控制器
        /// </summary>
        /// <param name="services"></param>
        internal static void AddControllers(this IServiceCollection services)
        {
            //添加进入services
            services.AddScoped<IControllersManager, ControllersManager>();
            services.AddScoped<IDelegateManager, ControllersManager>();
            services.AddScoped<IParamManager, ParamManager>();
        }

        /// <summary>
        /// 添加认证
        /// </summary>
        /// <param name="services"></param>
        public static void AddBotNameAuthentication(this IServiceCollection services)
        {
            services.AddScoped<IAuthentication, BotNameAuthentication>();
        }

        public static void AddUserAuthentication(this IServiceCollection services)
        {
            services.AddScoped<IAuthentication, UserAuthentication>();
            services.AddScoped<IAuthManager, AuthManager>();
        }
    }
}
