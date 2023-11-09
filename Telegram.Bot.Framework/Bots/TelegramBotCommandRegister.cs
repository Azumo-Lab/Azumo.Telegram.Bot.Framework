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
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Interfaces;

namespace Telegram.Bot.Framework.Bots
{
    internal class TelegramBotCommandRegister : ITelegramPartCreator
    {
        public void AddBuildService(IServiceCollection services)
        {

        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            _ = services.AddSingleton<IStartExec, RegisterBotCommand>();
        }

        private class RegisterBotCommand : IStartExec
        {
            public async Task Exec(ITelegramBotClient bot, IServiceProvider serviceProvider)
            {
                using (IServiceScope serviceScope = serviceProvider.CreateScope())
                {
                    IControllerManager controllerManager = serviceScope.ServiceProvider.GetRequiredService<IControllerManager>();
                    await bot.SetMyCommandsAsync(controllerManager.GetAllCommands().Select(x =>
                    {
                        return new Types.BotCommand()
                        {
                            Command = x.BotCommandName,
                            Description = x.Description,
                        };
                    }).ToList());
                }
            }
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        /// <summary>
        /// 添加基础的服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ITelegramBotBuilder RegisterBotCommand(this ITelegramBotBuilder builder)
        {
            return builder.AddTelegramPartCreator(new TelegramBotCommandRegister());
        }
    }
}
