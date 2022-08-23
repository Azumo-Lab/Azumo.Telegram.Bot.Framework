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
        private ISetUp SetISetUp;

        private TelegramBotManger() { }

        public static TelegramBotManger Config()
        {
            return new TelegramBotManger();
        }

        public TelegramBotManger Proxy(string URL, int Port, string Username = null, string Password = null)
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

        public TelegramBotManger SetToken(string Token)
        {
            ThrowHelper.ThrowIfNullOrEmpty(Token, $"{nameof(Token)} 为空");

            this.Token = Token;
            return this;
        }

        public TelegramBotManger SetUp(ISetUp SetUp)
        {
            SetISetUp = SetUp;
            return this;
        }

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
