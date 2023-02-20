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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Upgrader.Bot;
using Telegram.Bot.Upgrader.ParamMakers;

namespace Telegram.Bot.Upgrader.Controllers
{
    /// <summary>
    /// 管理本机Bot，管理升级启动停止的Controller
    /// </summary>
    internal class UPGradeController : TelegramController
    {
        private const string ASK_BOT_ID_MESSAGE = "请输入操作的BotID";

        private readonly IBotManager botManager;
        public UPGradeController(IBotManager botManager)
        {
            this.botManager = botManager;
        }

        /// <summary>
        /// 列出所有正在执行的Bot
        /// </summary>
        [Command(nameof(ListBots), CommandInfo = "列出所有的Bot")]
        public async Task ListBots()
        {
            List<(string, string)> botInfos = botManager.ListBotInfos();
            StringBuilder sb = new();
            sb.AppendLine("BotID  |  Bot信息  |   是否正在运行");
            foreach ((string botID, string botInfo) in botInfos)
                sb.AppendLine($"{botID} | {botInfo}  |  {""}");
            await Context.SendTextMessage(sb.ToString());
        }

        /// <summary>
        /// 停止指定的Bot
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        [Command(nameof(StopBot), CommandInfo = "停止指定Bot")]
        public async void StopBot(
            [Param(ASK_BOT_ID_MESSAGE)] string botID)
        {
            if (botManager.StopBot(botID))
                await Context.SendTextMessage($"已停止Bot {botID}");
            else
                await Context.SendTextMessage($"未找到ID： {botID}");
        }

        /// <summary>
        /// 升级指定Bot
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        /// <param name="zipArchive">Bot升级的压缩文件</param>
        [Command(nameof(UPGradeBot), CommandInfo = "升级替换指定Bot")]
        public async void UPGradeBot(
            [Param(ASK_BOT_ID_MESSAGE)] string botID,
            [Param("请上传Bot程序", CustomParamMaker = typeof(ZipFileCatch))] ZipArchive zipArchive)
        {
            if (botManager.UpdateBot(botID, zipArchive))
                await Context.SendTextMessage($"已升级Bot {botID}");
            else
                await Context.SendTextMessage($"未找到ID： {botID}");
        }

        /// <summary>
        /// 添加Bot
        /// </summary>
        /// <param name="botInfo">bot信息</param>
        /// <param name="zipArchive">Bot执行程序的压缩文件</param>
        [Command(nameof(AddBot), CommandInfo = "添加一个全新的Bot")]
        public async void AddBot(
            [Param("请输入Bot信息")]string botInfo,
            [Param("请上传Bot程序", CustomParamMaker = typeof(ZipFileCatch))] ZipArchive zipArchive)
        {
            try
            {
                botManager.AddBot(botInfo, zipArchive);

                await Context.SendTextMessage("操作成功");
            }
            catch (ArgumentException)
            {
                await Context.SendTextMessage("您的操作速度已经达到惊人的0.001秒的闪电速度，请休息一下，再来操作");
            }
            catch (Exception)
            {
                await Context.SendTextMessage("发生了未知错误");
            }
        }

        /// <summary>
        /// 启动指定Bot
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        [Command(nameof(StartBot), CommandInfo = "启动指定Bot")]
        public async void StartBot(
            [Param(ASK_BOT_ID_MESSAGE)] string botID)
        {
            if (botManager.StartBot(botID))
                await Context.SendTextMessage($"已启动Bot {botID}");
            else
                await Context.SendTextMessage($"未找到ID： {botID}");
        }

        /// <summary>
        /// 删除一个Bot
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        [Command(nameof(RemoveBot), CommandInfo = "删除指定Bot")]
        public async void RemoveBot(
            [Param(ASK_BOT_ID_MESSAGE)]string botID)
        {
            if (botManager.RemoveBot(botID))
                await Context.SendTextMessage($"已删除Bot {botID}");
            else
                await Context.SendTextMessage($"未找到ID： {botID}");
        }
    }
}
