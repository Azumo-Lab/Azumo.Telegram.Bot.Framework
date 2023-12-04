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
using Telegram.Bot.Framework.Bots;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// <see cref="ITelegramBotBuilder"/> 接口的实现类
    /// </summary>
    public class TelegramBuilder : ITelegramBotBuilder
    {
        /// <summary>
        /// 运行时期的服务
        /// </summary>
        private readonly IServiceCollection __RuntimeService = new ServiceCollection();

        /// <summary>
        /// 创建时期的服务
        /// </summary>
        private readonly IServiceCollection __BuildServices = new ServiceCollection();

        /// <summary>
        /// 添加的可扩展内容接口 <see cref="ITelegramPartCreator"/>
        /// </summary>
        private readonly List<ITelegramPartCreator> __BuildPartCreator = [];

        /// <summary>
        /// 开始创建 <see cref="ITelegramBotBuilder"/> 接口
        /// </summary>
        /// <remarks>
        /// 创建 <see cref="ITelegramPartCreator"/> 接口时候，会添加一个Bot执行的基础服务
        /// </remarks>
        /// <returns>添加基础服务后的 <see cref="ITelegramBotBuilder"/> 接口</returns>
        public static ITelegramBotBuilder Create()
        {
            return new TelegramBuilder().AddBasic();
        }

        /// <summary>
        /// 初始化，不对外暴露
        /// </summary>
        private TelegramBuilder() { }

        /// <summary>
        /// 开始创建 <see cref="ITelegramBot"/> 对象
        /// </summary>
        /// <remarks>
        /// 先执行创建时期的服务创建，结束之后，进行运行时期的服务创建
        /// </remarks>
        /// <returns>添加完成各类服务的 <see cref="ITelegramBot"/> 接口</returns>
        public ITelegramBot Build()
        {
            IServiceProvider buildService = __BuildServices.BuildServiceProvider();
            foreach (ITelegramPartCreator telegramPartCreator in __BuildPartCreator)
                telegramPartCreator.Build(__RuntimeService, buildService);

            IServiceProvider serviceProvider = __RuntimeService.BuildServiceProvider();
            return serviceProvider.GetService<ITelegramBot>();
        }

        /// <summary>
        /// 添加 <see cref="ITelegramPartCreator"/> 接口示例
        /// </summary>
        /// <remarks>
        /// 通过实现并添加 <see cref="ITelegramPartCreator"/> 接口，来进行服务的创建和扩展
        /// </remarks>
        /// <param name="telegramPartCreator">可扩展内容接口实例</param>
        /// <returns>返回添加后的本接口 <see cref="ITelegramBotBuilder"/></returns>
        public ITelegramBotBuilder AddTelegramPartCreator(ITelegramPartCreator telegramPartCreator)
        {
            telegramPartCreator.AddBuildService(__BuildServices);
            __BuildPartCreator.Add(telegramPartCreator);
            return this;
        }
    }
}
