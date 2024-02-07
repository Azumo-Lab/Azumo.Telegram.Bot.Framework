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

using Azumo.SuperExtendedFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller.BotBuilder;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Framework.SimpleAuthentication;

namespace Telegram.Bot.Example;

internal class Program
{
    public static void Main(string[] args)
    {
        var dic = ArgsHelper.ToDictionary(args);
        if (!dic.TryGetValue("-setting", out var settingPath))
            ArgumentException.ThrowIfNullOrEmpty(settingPath, "请添加启动参数 -setting ，后面添加配置文件路径");

        // 创建ITelegramBotBuilder接口
        var telegramBot = TelegramBot.CreateBuilder()
            .UseController()
            // 添加配置文件
            .AddConfiguration<AppSetting>(settingPath)
            // 使用Token
            .UseToken<AppSetting>(x => x.Token)
            // 使用Clash默认代理
            .UseClashDefaultProxy()
            // 添加一个控制台Log
            .AddSimpleConsole()
            // 注册Bot指令
            .RegisterBotCommand()
            // 添加用户认证服务
            .UseSimpleAuthentication()
            // 添加指令
            .AddCommand([BotCommand("Func", Description = "Func测试")] async (TelegramUserContext telegramUserChatContext) =>
            {
                _ = await telegramUserChatContext.BotClient.SendTextMessageAsync(telegramUserChatContext.ScopeChatID, "Func 测试");
                await Task.CompletedTask;
            })
            // 创建机器人接口
            .Build();

        // 启动Bot
        var task = telegramBot.StartAsync(true);
        task.Wait();
    }
}

[TelegramController]
public class TestController
{
    [BotCommand("/Admin")]
    public static async Task HelloWorld() => 
        await Task.CompletedTask;
}
