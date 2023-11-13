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

using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Bots
{
    /// <summary>
    /// 添加Token的设置
    /// </summary>
    internal class TelegramToken : ITelegramPartCreator
    {
        private readonly string __Token;
        public TelegramToken(string token)
        {
            __Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public void AddBuildService(IServiceCollection services)
        {

        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            HttpClient proxy;
            _ = (proxy = builderService.GetService<HttpClient>()) != null
                ? services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(__Token, proxy))
                : services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(__Token));
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
    }
}
