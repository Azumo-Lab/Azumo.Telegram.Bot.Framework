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
using System.IO.Compression;
using System.Text;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract.Commands;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Upgrader.Bot;
using Telegram.Bot.Upgrader.ParamMakers;

namespace Telegram.Bot.Upgrader.Controllers
{
    /// <summary>
    /// 管理本机Bot，管理升级启动停止的Controller
    /// </summary>
    [Authentication(AuthenticationRole.BotAdmin)]
    internal class UPGradeController : TelegramController
    {
        private const string ASK_BOT_ID_MESSAGE = "请输入操作的BotID";

        private readonly IBotManager botManager;
        public UPGradeController(IBotManager botManager)
        {
            this.botManager = botManager;
        }

        /// <summary>
        /// 开始菜单
        /// </summary>
        /// <returns></returns>
        [Command(nameof(Start))]
        public async Task Start()
        {
            ITelegramCommandsManager commandManager = Context.UserScope.GetRequiredService<ITelegramCommandsManager>();

            string message = "你好，这里是升级管理机器人：";

            message += Environment.NewLine;
            message += Environment.NewLine;

            message += commandManager.GetBotCommandsString();

            message += Environment.NewLine;
            message += "项目地址：https://github.com/Azumo-Lab/Telegram.Bot.Framework/";

            await Context.SendTextMessage(message);
        }

        /// <summary>
        /// 列出所有正在执行的Bot
        /// </summary>
        [Command(nameof(ListBots), CommandInfo = "列出所有的Bot")]
        public async Task ListBots()
        {
            List<BotInfoModel> botInfos = botManager.ListBotInfos();
            StringBuilder sb = new();
            _ = sb.AppendLine("BotID  |  Bot信息  |   是否正在运行");
            foreach (BotInfoModel botInfo in botInfos)
                _ = sb.AppendLine($"{botInfo.BotID} | {botInfo.BotInfo}  |  {(botInfo.IsRunning ? "运行中" : "停止")}");
            if (!botInfos.Any())
                _ = sb.AppendLine("未找到任何信息");
            await Context.SendTextMessage(sb.ToString());
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
        /// 更新指定Bot的信息
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        /// <param name="botInfo">bot信息</param>
        [Command(nameof(UpdateInfo), CommandInfo = "更新指定Bot的信息")]
        public async void UpdateInfo(
            [Param(ASK_BOT_ID_MESSAGE)] string botID,
            [Param("请输入Bot信息")] string botInfo)
        {
            if (botManager.UpdateBotInfo(botID, botInfo))
                await Context.SendTextMessage($"已更新Bot {botID}");
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
            [Param("请输入Bot信息")] string botInfo,
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
        /// 删除一个Bot
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        [Command(nameof(RemoveBot), CommandInfo = "删除指定Bot")]
        public async void RemoveBot(
            [Param(ASK_BOT_ID_MESSAGE)] string botID)
        {
            if (botManager.RemoveBot(botID))
                await Context.SendTextMessage($"已删除Bot {botID}");
            else
                await Context.SendTextMessage($"未找到ID： {botID}");
        }

        /// <summary>
        /// 重新启动Bot
        /// </summary>
        /// <param name="botID"></param>
        [Command(nameof(ReStart), CommandInfo = "重新启动Bot")]
        public async void ReStart(
            [Param(ASK_BOT_ID_MESSAGE)] string botID)
        {
            bool result = botManager.StopBot(botID);
            if (!result || !botManager.StartBot(botID))
                await Context.SendTextMessage($"重启失败，BotID： {botID}"); 
            else
                await Context.SendTextMessage($"已重新启动Bot {botID}");
        }
    }
}
