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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;

namespace Telegram.Bot.Framework
{
    public class TelegramBotManger
    {
        private HttpClient HttpClient;
        private string Token;
        private IConfig SetISetUp;
        private string BotName;

        /// <summary>
        /// 初始化
        /// </summary>
        private TelegramBotManger() { }

        /// <summary>
        /// 创建一个新的配置对象
        /// </summary>
        /// <returns></returns>
        public static TelegramBotManger CreateConfig()
        {
            return new TelegramBotManger();
        }

        /// <summary>
        /// 设置Bot名称
        /// </summary>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public TelegramBotManger SetBotName(string Name)
        {
            BotName = Name;
            if (string.IsNullOrEmpty(BotName))
                throw new ArgumentNullException($"{nameof(BotName)} : is Null or Empty");
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
            ThrowHelper.ThrowIfNullOrEmpty(URL);
            ThrowHelper.ThrowIfZeroAndDown(Port);

            WebProxy webProxy = new WebProxy(Host: URL, Port: Port);
            if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Password))
            {
                webProxy.Credentials = new NetworkCredential(Username, Password);
            }
            HttpClient = new HttpClient(
                new HttpClientHandler { Proxy = webProxy, UseProxy = true, }
            );
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
        public TelegramBotManger SetConfig<ConfigType>(ConfigType SetUp) where ConfigType : IConfig
        {
            SetISetUp = SetUp;
            return this;
        }

        /// <summary>
        /// 添加设置
        /// </summary>
        /// <param name="SetUp"></param>
        /// <returns></returns>
        public TelegramBotManger SetConfig<ConfigType>() where ConfigType : IConfig
        {
            SetConfig((IConfig)Activator.CreateInstance(typeof(ConfigType)));
            return this;
        }

        /// <summary>
        /// 创建一个Bot对象
        /// </summary>
        /// <returns></returns>
        public TelegramBot Build()
        {
            TelegramBotClient botClient;
            if (HttpClient != null)
                botClient = new TelegramBotClient(Token, HttpClient);
            else
                botClient = new TelegramBotClient(Token);
            return new TelegramBot(botClient, SetISetUp, BotName);
        }
    }
}
