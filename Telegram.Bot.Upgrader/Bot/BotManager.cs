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

using System.IO.Compression;
using System.Text;
using Telegram.Bot.Framework.Abstract.Security;

namespace Telegram.Bot.Upgrader.Bot
{
    /// <summary>
    /// Bot管理
    /// </summary>
    internal class BotManager : IBotManager, IDisposable
    {
        // 存放Bot信息的文件夹
        private const string BOT_INFO_FOLDER = "BotInfos";
        // Bot信息文件扩展名
        private const string BOT_INFO_PARAMNAME = ".botinfo";
        // Bot执行文件目录
        private const string BOT_FILE_ROOT = "BotFileRoot";

        private static readonly Dictionary<string, BotInfoModel> BotInfo;
        static BotManager()
        {
            BotInfo = new Dictionary<string, BotInfoModel>();
            _ = Directory.CreateDirectory(BOT_INFO_FOLDER);
            // 从文件中加载Bot的信息
            foreach (string filePath in Directory.GetFiles(BOT_INFO_FOLDER, $"*{BOT_INFO_PARAMNAME}", SearchOption.TopDirectoryOnly))
            {
                BotInfoModel botInfoModel = new BotInfoModel().Decode(File.ReadAllText(filePath));
                botInfoModel.IsRunning = false;
                BotInfo.Add(botInfoModel.BotID!, botInfoModel);
            }
        }

        private readonly IProcessManager processManager;

        public BotManager(IProcessManager processManager)
        {
            this.processManager = processManager;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// 添加一个Bot
        /// </summary>
        /// <param name="botInfo">Bot信息</param>
        /// <param name="botFile">Bot的可执行文件压缩包</param>
        public void AddBot(string botInfo, ZipArchive botFile)
        {
            // 生成ID
            string botID = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            // 创建存放文件夹
            DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.Combine(BOT_FILE_ROOT, botID));
            // 讲压缩文件解压
            botFile.ExtractToDirectory(directoryInfo.FullName, true);
            botFile.Dispose();
            // 找可执行文件
            FileInfo fileInfo = directoryInfo.GetFiles("*.exe", SearchOption.AllDirectories).Single();

            BotInfoModel model = new()
            {
                BotID = botID,
                BotInfo = botInfo,
                BotFilePath = directoryInfo.FullName,
                EXEPath = fileInfo.FullName,
            };
            BotInfo.Add(botID, model);
            SaveBotInfo();
        }

        /// <summary>
        /// 列出所有的Bot
        /// </summary>
        /// <returns>Bot的信息列表</returns>
        public List<BotInfoModel> ListBotInfos()
        {
            return BotInfo.Select(x => x.Value).ToList();
        }

        /// <summary>
        /// 移除Bot的信息，包括执行文件
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        /// <returns>是否执行成功</returns>
        public bool RemoveBot(string botID)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfoModel botInfo = BotInfo[botID];

            _ = StopBot(botInfo.BotID!);
            Directory.Delete(botInfo.BotFilePath!, true);

            _ = BotInfo.Remove(botID);
            SaveBotInfo();

            return true;
        }

        /// <summary>
        /// 启动Bot
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        /// <returns>是否执行成功</returns>
        public bool StartBot(string botID)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfoModel botInfo = BotInfo[botID];
            if (processManager.IsRunning(botInfo.BotID!))
                return true;
            return File.Exists(botInfo.EXEPath) && (botInfo.IsRunning = processManager.Start(botID, botInfo.EXEPath));
        }

        /// <summary>
        /// 停止执行Bot
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        /// <returns>是否执行成功</returns>
        public bool StopBot(string botID)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfoModel botInfo = BotInfo[botID];
            if (!processManager.IsRunning(botInfo.BotID!))
                return true;
            botInfo.IsRunning = false;
            return processManager.Stop(botID);
        }

        /// <summary>
        /// 升级Bot的执行文件
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        /// <param name="botFile">升级的文件</param>
        /// <returns>是否执行成功</returns>
        public bool UpdateBot(string botID, ZipArchive botFile)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfoModel botInfo = BotInfo[botID];

            _ = StopBot(botInfo.BotID!);
        RELOAD:
            try
            {
                if (Directory.Exists(botInfo.BotFilePath))
                    Directory.Delete(botInfo.BotFilePath, true);
            }
            catch (Exception)
            {
                Thread.Sleep(2000);
                goto RELOAD;
            }

            DirectoryInfo directoryInfo = Directory.CreateDirectory(botInfo.BotFilePath!);
            botFile.ExtractToDirectory(directoryInfo.FullName, true);

            return true;
        }

        /// <summary>
        /// 更新Bot信息
        /// </summary>
        /// <param name="botID">Bot的ID</param>
        /// <param name="botInfo">Bot的信息</param>
        /// <returns>是否执行成功</returns>
        public bool UpdateBotInfo(string botID, string botInfo)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfo[botID].BotInfo = botInfo;

            SaveBotInfo();

            return true;
        }

        /// <summary>
        /// 保存Bot信息
        /// </summary>
        private static void SaveBotInfo()
        {
            // 将信息更新到文件
            BotInfo.Select(x => x.Value).ToList().ForEach(model =>
            {
                string filePath = Path.Combine(BOT_INFO_FOLDER, $"{model.BotID}{BOT_INFO_PARAMNAME}");
                File.WriteAllText(filePath, model.Encrypt(), Encoding.UTF8);
            });
            // 删除旧的文件
            foreach (string filePath in Directory.GetFiles(BOT_INFO_FOLDER))
                if (!BotInfo.ContainsKey(Path.GetFileNameWithoutExtension(filePath)))
                    File.Delete(filePath);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            SaveBotInfo();
        }
    }

    /// <summary>
    /// Bot的相关信息
    /// </summary>
    public class BotInfoModel : EncryptDecodeBase<BotInfoModel>
    {
        /// <summary>
        /// Bot的ID
        /// </summary>
        public string? BotID { get; set; }

        /// <summary>
        /// Bot的信息
        /// </summary>
        public string? BotInfo { get; set; }

        /// <summary>
        /// Bot的文件夹路径
        /// </summary>
        public string? BotFilePath { get; set; }

        /// <summary>
        /// Bot执行文件路径
        /// </summary>
        public string? EXEPath { get; set; }

        /// <summary>
        /// Bot是否在运行中
        /// </summary>
        public bool IsRunning { get; set; }
    }
}
