//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.Components;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Example.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class LetsPlayGame : TelegramController
    {
        //[Command(nameof(PlayOnline), CommandInfo = "一起在线玩游戏")]
        public async Task PlayOnline()
        {
            await Context.SendTextMessage("正在与管理员连线中...");

            IUserManager userManager = Context.UserScope.GetService<IUserManager>();
            IContact contact = Context.UserScope.GetService<IContact>();

            await Context.SendTextMessage(userManager.Admin, "用户来了联机请求。");
            await Context.SendTextMessage("成功与管理员取得联系");

            contact.AddContact(Context, userManager.Admin);

            await Context.SendTextMessage("请玩游戏，选择你的选项：", new List<InlineButtons>
            {
                InlineButtons.WithCallback("石头", async context =>
                {
                    START:
                    IContact contact = context.UserScope.GetService<IContact>();
                    while (contact.GetContact(context).UserData.TryGetValue("JanKen", out string item))
                    {

                    }
                    goto START;
                }),
                InlineButtons.WithCallback("剪子", context =>
                {

                }),
                InlineButtons.WithCallback("包袱", context =>
                {

                }),
            });
            await Context.SendTextMessage(userManager.Admin, "请玩游戏，选择你的选项：", new List<InlineButtons>
            {
                InlineButtons.WithCallback("石头", context =>
                {

                }),
                InlineButtons.WithCallback("剪子", context =>
                {

                }),
                InlineButtons.WithCallback("包袱", context =>
                {

                }),
            });
        }

        public static async Task JanKen(string item, string tekiItem, TelegramContext context)
        {

        }
    }
}
