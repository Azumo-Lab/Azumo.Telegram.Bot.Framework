using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Telegram.Bot.Framework.FrameworkHelper;

namespace Telegram.Bot.Framework
{
    public class TelegramBotManger
    {
        private HttpClient HttpClient;
        private string Token;
        private IConfig SetISetUp;

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
        public TelegramBotManger SetConfig(IConfig SetUp)
        {
            SetISetUp = SetUp;
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

            return new TelegramBot(botClient, SetISetUp);
        }
    }
}
