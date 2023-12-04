//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Bots
{
    /// <summary>
    /// 添加Token的设置
    /// </summary>
    [DebuggerDisplay("Token 设置：{__Token}")]
    internal class TelegramToken : ITelegramPartCreator
    {
        /// <summary>
        /// 存储的Token
        /// </summary>
        private string __Token;

        /// <summary>
        /// 从配置文件中获取的Token
        /// </summary>
        private readonly Func<IConfiguration, string> __TokenFunc;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TelegramToken(string token)
        {
            __Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TelegramToken(Func<IConfiguration, string > configuration)
        {
            __TokenFunc = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void AddBuildService(IServiceCollection services)
        {
            if (__Token == null)
            {
                const string AppSettingPath = "appsetting.json";
                if (File.Exists(AppSettingPath))
                    services.TryAddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile(AppSettingPath).Build());
            }
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            // 获取Token
            IConfiguration configuration = builderService.GetService<IConfiguration>();
            __Token ??= __TokenFunc?.Invoke(configuration ?? 
                throw new NullReferenceException($"未找到配置文件，请在构建Bot的时候，" +
                $"调用 {nameof(ITelegramBotBuilder)} 接口的" +
                $" {nameof(TelegramBuilderExtensionMethods.AddConfiguration)} 扩展方法来添加配置文件"));

            // 检查Token
            if (string.IsNullOrEmpty(__Token))
                throw new NullReferenceException("Token 为 Null 或者是空字符串：\"\"");

            // 通过Token 设置 ITelegramBotClient 
            HttpClient proxy;
            if ((proxy = builderService.GetService<HttpClient>()) != null)
                services.TryAddSingleton<ITelegramBotClient>(new TelegramBotClient(__Token, proxy));
            else
                services.TryAddSingleton<ITelegramBotClient>(new TelegramBotClient(__Token));
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        /// <summary>
        /// 添加 Token
        /// </summary>
        /// <remarks>
        /// 添加 BotFather 发行的机器人Token，没有Token，机器人将无法正常启动
        /// </remarks>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ITelegramBotBuilder UseToken(this ITelegramBotBuilder builder, string token)
        {
            return builder.AddTelegramPartCreator(new TelegramToken(token));
        }

        /// <summary>
        /// 添加 Token
        /// </summary>
        /// <remarks>
        /// 添加 BotFather 发行的机器人Token，没有Token，机器人将无法正常启动 <br></br>
        /// 这个方法是通过配置文件 <see cref="IConfiguration"/> 来取得 Token 数据
        /// </remarks>
        /// <param name="builder"></param>
        /// <param name="tokenFunc"></param>
        /// <returns></returns>
        public static ITelegramBotBuilder UseToken(this ITelegramBotBuilder builder, Func<IConfiguration, string> tokenFunc)
        {
            return builder.AddTelegramPartCreator(new TelegramToken(tokenFunc));
        }
    }
}
