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

using System.Diagnostics;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Exec;

namespace Telegram.Bot.Framework.Bots;

[DebuggerDisplay("自动注册Bot指令服务")]
internal class TelegramBotCommandRegister : ITelegramPartCreator
{
    public void AddBuildService(IServiceCollection services)
    {

    }

    public void Build(IServiceCollection services, IServiceProvider builderService) =>
        _ = services.AddSingleton<IStartExec, RegisterBotCommand>();

    /// <summary>
    /// Bot启动任务
    /// </summary>
    private class RegisterBotCommand : IStartExec
    {
        public async Task Exec(ITelegramBotClient bot, IServiceProvider serviceProvider)
        {
            // 开始注册
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var controllerManager = serviceScope.ServiceProvider.GetRequiredService<IControllerManager>();
                await bot.SetMyCommandsAsync(controllerManager.GetAllCommands().Select(x => new Types.BotCommand()
                {
                    Command = x.BotCommandName,
                    Description = x.Description,
                }).ToList());
            }
        }
    }
}

public static partial class TelegramBuilderExtensionMethods
{
    /// <summary>
    /// 收集Bot内的指令，并注册到Telegram中
    /// </summary>
    /// <remarks>
    /// 用于收集 <see cref="TelegramController"/> 中使用 <see cref="BotCommandAttribute"/> 的指令。
    /// 并将相关信息注册到Telegram中。
    /// </remarks>
    /// <param name="builder">创建接口</param>
    /// <returns>服务添加后的创建接口</returns>
    public static ITelegramBotBuilder RegisterBotCommand(this ITelegramBotBuilder builder) => builder.AddTelegramPartCreator(new TelegramBotCommandRegister());
}
