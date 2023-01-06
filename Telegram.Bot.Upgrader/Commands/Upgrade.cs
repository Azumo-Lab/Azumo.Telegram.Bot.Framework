////  <Telegram.Bot.Framework>
////  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
////
////  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
////  it under the terms of the GNU General Public License as published by
////  the Free Software Foundation, either version 3 of the License, or
////  (at your option) any later version.
////
////  This program is distributed in the hope that it will be useful,
////  but WITHOUT ANY WARRANTY; without even the implied warranty of
////  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////  GNU General Public License for more details.
////
////  You should have received a copy of the GNU General Public License
////  along with this program.  If not, see <https://www.gnu.org/licenses/>.

//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.Loader;
//using System.Threading.Tasks;
//using Telegram.Bot.Framework;
//using Telegram.Bot.Framework.Abstract;
//using Telegram.Bot.Framework.TelegramAttributes;
//using Telegram.Bot.Upgrader.Makers;

//namespace Telegram.Bot.Upgrader.Commands
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class Upgrade : TelegramController
//    {
//        private static Process process;

//        private IServiceProvider serviceProvider;
//        public Upgrade(IServiceProvider serviceProvider)
//        {
//            this.serviceProvider = serviceProvider;
//        }

//        [Authentication(AuthenticationRole.ADMIN)]
//        [Command("Upgrade", CommandInfo = "升级Bot")]
//        public async Task BotUpgrade([Param("请发送升级文件", CustomParamMaker = typeof(ZipFileCatch))] ZipArchive zipArchive)
//        {
//            if (Directory.Exists("BotEXE"))
//            {
//                Directory.Delete("BotEXE", true);
//            }
//            var zipfile = Directory.CreateDirectory("BotEXE");
//            zipArchive.ExtractToDirectory("BotEXE", true);

//            FileInfo exefile = zipfile.GetFiles().Where(x=>x.Extension.ToLower() == ".exe").FirstOrDefault();
//            if (exefile == null)
//            {
//                await Context.SendTextMessage("启动失败");
//                return;
//            }

//            if (process != null && !process.HasExited)
//                process.Kill();

//            process = new Process();
//            process.StartInfo = new ProcessStartInfo
//            {
//                FileName = exefile.FullName,
//            };
//            process.Start();

//            await Context.SendTextMessage("启动成功");
//        }

//        [Authentication(AuthenticationRole.ADMIN)]
//        [Command("Stop", CommandInfo = "停止Bot")]
//        public async Task Stop()
//        {
//            if (process != null && !process.HasExited)
//                process.Kill();

//            await Context.SendTextMessage("停止成功");
//        }

//        [Authentication(AuthenticationRole.ADMIN)]
//        [Command("Message")]
//        public async Task TestMessage()
//        {
//            await Context.SendTextMessage("管理员");
//        }
//    }
//}
