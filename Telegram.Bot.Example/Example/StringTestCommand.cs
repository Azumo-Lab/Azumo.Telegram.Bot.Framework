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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Example.Example
{
    /// <summary>
    /// 
    /// </summary>
    public class StringTestCommand : TelegramController
    {
        private IServiceProvider serviceProvider;
        public StringTestCommand(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [DefaultMessageType(Types.Enums.MessageType.Text)]
        public async Task StringCatch()
        {
            string message = string.Empty;
            string msg = Context.Update.Message.Text;

            if (msg.ToUpper().Contains("TEST"))
            {
                await Context.SendTextMessage("我也来个Test (*^_^*)， Test，TEst，teST");
                return;
            }

            if (long.TryParse(msg, out long l))
            {
                await Context.SendTextMessage("纯数字？反转啦！！");
                await Context.SendTextMessage(string.Join("", msg.Reverse()));
                await Context.SendTextMessage("12345，上山打老虎是吧");
                return;
            }

            if (ISCHAR(msg))
            {
                await Context.SendTextMessage("你输入的这一串是什么，看不懂啦");
                await Context.SendTextMessage("你试试输入纯数字和TEST。");
                await Context.SendTextMessage("试试输入：河南");
                return;
            }

            if (msg.Contains("河南"))
            {
                await Context.SendTextMessage("我们河南人就是神，创造不朽的神话！！");
                return;
            }

            await Context.SendTextMessage("你试试输入纯数字和TEST。");
            await Context.SendTextMessage("试试输入：河南");
        }

        private bool ISCHAR(string c)
        {
            foreach (var item in c)
            {
                if (item is >= 'A' and <= 'Z' or >= 'a' and <= 'z')
                {
                    return true;
                }
            }
            return false;
        }

        [DefaultMessageType(Types.Enums.MessageType.Photo)]
        public async Task PhotoCatch()
        {
            var photoSize = Context.Update.Message.Photo.ToList().OrderBy(x => x.FileSize).LastOrDefault();
            Directory.CreateDirectory("TESTPIC");
            using (StreamWriter sw = new StreamWriter(new FileStream($"TESTPIC/{photoSize.FileId}", FileMode.OpenOrCreate)))
            {
                sw.Write(JsonConvert.SerializeObject(photoSize));
            }
            await Context.SendTextMessage("已保存");
            await Context.SendTextMessage(
@"
你可以使用 /GetAllSendPhoto 获取全部图片，
也可以使用 /GetSendPhoto 随机获取一张图片");
        }

        [Command("Stop", CommandInfo = "停止这个机器人")]
        public async Task ExitExe([Param("密码：")]string password)
        {
            var Secrets = new ConfigurationBuilder().AddUserSecrets("98def42c-77dc-41cb-abf6-2c402535f4cb").Build();
            string Password = Secrets.GetSection("PASSWORD").Value;
            if (password == Password)
            {
                TelegramBot bot = serviceProvider.GetService<TelegramBot>();
                bot.Stop();
            }
            await Task.Delay(1);
        }

        [Authentication(AuthenticationRole.ADMIN)]
        [Command("AdminTest", CommandInfo = "测试管理员权限认证")]
        public async Task AdminTest()
        {
            await Context.SendTextMessage("你已经通过了管理员认证");
        }
    }
}
