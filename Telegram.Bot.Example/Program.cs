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

using Azumo.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Bots;
using Telegram.Bot.Framework.UserAuthentication;

namespace Telegram.Bot.Example
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dic = ArgsHelper.ToDictionary(args);
            if (!dic.TryGetValue("-setting", out var settingPath))
                ArgumentException.ThrowIfNullOrEmpty(settingPath, "请添加启动参数 -setting ，后面添加配置文件路径");

            // 创建ITelegramBotBuilder接口
            var telegramBot = TelegramBuilder.Create()
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
                .AddUserAuthentication(["admin", "user"])
                // 创建机器人接口
                .Build();

            // 启动Bot
            var task = telegramBot.StartAsync();
            task.Wait();
        }
    }

    public class TestController : TelegramController
    {
        [BotCommand(Description = "进行管理员认证")]
        public async Task Login([Param(Name = "密码")]string password)
        {
            var userManager = Chat.UserScopeService.GetService<IUserManager>();
            userManager.OnSignIn += UserManager_OnSignIn;
            await userManager.UserSignIn(Chat.User, password);
            await userManager.UserRole(Chat.User, "admin");
            userManager.OnSignIn -= UserManager_OnSignIn;
            await SendMessage("已赋予管理员权限");
        }

        private void UserManager_OnSignIn(object sender, SignInArgs e)
        {
            var appSetting = Chat.UserScopeService.GetService<AppSetting>();
            e.PasswordHash = PasswordHelper.Hash(appSetting.AdminPassword);
            e.UserRoles.Add("admin");
        }

        [Authenticate("admin")]
        [BotCommand("/Admin")]
        public async Task Test2()
        {
            await SendMessage("管理员");
            await SendMessage("欢迎管理员！！");
        }
    }
}
