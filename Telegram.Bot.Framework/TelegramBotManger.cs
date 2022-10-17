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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.Models;

namespace Telegram.Bot.Framework
{
    public sealed class TelegramBotManger
    {
        private static HashSet<string> Tokens = new HashSet<string>();  //判断重复Token
        private string Token;                                           //Token
        private string BotName;                                         //Bot名称

        private readonly IServiceCollection services;

        /// <summary>
        /// 初始化
        /// </summary>
        private TelegramBotManger() 
        {
            services = new ServiceCollection();
            services.AddScoped<BotInfos>();
        }

        /// <summary>
        /// 创建一个新的配置对象
        /// </summary>
        /// <returns></returns>
        public static TelegramBotManger Create()
        {
            return new TelegramBotManger();
        }

        /// <summary>
        /// 设置Bot名称
        /// </summary>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public TelegramBotManger SetBotName(string BotName)
        {
            if (string.IsNullOrEmpty(BotName))
                throw new ArgumentNullException(nameof(BotName));
            this.BotName = BotName;
            return this;
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="URL">代理地址</param>
        /// <param name="Port">代理端口</param>
        /// <param name="Username">代理用户名</param>
        /// <param name="Password">代理密码</param>
        /// <returns></returns>
        public TelegramBotManger SetProxy(string URL, int Port, string Username = null, string Password = null)
        {
            ThrowHelper.ThrowIfNullOrEmpty(URL);        //检查URL是否是空
            ThrowHelper.ThrowIfZeroAndDown(Port);       //检查端口是否是0或小于0

            WebProxy webProxy = new(Host: URL, Port: Port);
            if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Password))
            {
                webProxy.Credentials = new NetworkCredential(Username, Password);
            }
            HttpClient client = new HttpClient(
                new HttpClientHandler { Proxy = webProxy, UseProxy = true, }
            );

            services.AddSingleton<HttpClient>(client);

            return this;
        }

        /// <summary>
        /// 设置Token
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public TelegramBotManger SetToken(string Token)
        {
            ThrowHelper.ThrowIfNullOrEmpty(Token, $"{nameof(Token)} 为空");

            this.Token = Token;
            return this;
        }

        /// <summary>
        /// 添加设置
        /// </summary>
        /// <param name="SetUp"></param>
        /// <returns></returns>
        public TelegramBotManger AddConfig<ConfigType>() where ConfigType : class, IConfig
        {
            services.AddScoped<IConfig, ConfigType>();
            return this;
        }

        /// <summary>
        /// 添加设置
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public TelegramBotManger AddConfig(Action<IServiceCollection> action)
        {
            services.AddScoped<IConfig, DefaultConfig>(x =>
            {
                DefaultConfig defConf = new();
                defConf.SetAction(action);
                return defConf;
            });
            return this;
        }

        public TelegramBotManger AddBackgroundTask(Action action)
        {
            Task.Run(action);
            return this;
        }

        /// <summary>
        /// 创建一个Bot对象
        /// </summary>
        /// <returns></returns>
        public TelegramBot Build()
        {
            ThrowHelper.ThrowIfNullOrEmpty(Token);

            if (Tokens.Contains(Token))
                throw new Exception($"重复的Token : {Token}");
            Tokens.Add(Token);

            services.AddScoped<IConfig, FrameworkConfig>();
            services.AddScoped(x => new TelegramBot(x));

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                BotInfos info = serviceProvider.GetService<BotInfos>();
                info.BotName = BotName;
                info.Token = Token;

                return serviceProvider.GetService<TelegramBot>();
            }
        }
    }
}
