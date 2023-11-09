//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using Telegram.Bot.Framework.Bots;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class TelegramBuilder : ITelegramBotBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceCollection __RuntimeService = new ServiceCollection();

        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceCollection __BuildServices = new ServiceCollection();

        /// <summary>
        /// 
        /// </summary>
        private readonly List<ITelegramPartCreator> __BuildPartCreator = new();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITelegramBotBuilder Create()
        {
            return new TelegramBuilder().AddBasic();
        }

        /// <summary>
        /// 
        /// </summary>
        private TelegramBuilder() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ITelegramBot Build()
        {
            IServiceProvider buildService = __BuildServices.BuildServiceProvider();
            foreach (ITelegramPartCreator telegramPartCreator in __BuildPartCreator)
                telegramPartCreator.Build(__RuntimeService, buildService);

            IServiceProvider serviceProvider = __RuntimeService.BuildServiceProvider();
            return serviceProvider.GetService<ITelegramBot>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramPartCreator"></param>
        /// <returns></returns>
        public ITelegramBotBuilder AddTelegramPartCreator(ITelegramPartCreator telegramPartCreator)
        {
            telegramPartCreator.AddBuildService(__BuildServices);
            __BuildPartCreator.Add(telegramPartCreator);
            return this;
        }
    }
}
