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

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Security;

namespace Telegram.Bot.Upgrader.Bot
{
    /// <summary>
    /// 
    /// </summary>
    internal class BotManager : IBotManager, IDisposable
    {
        private const string BOT_INFO_FOLDER = "BotInfos";
        private const string BOT_INFO_PARAMNAME = ".botinfo";

        private const string BOT_FILE_ROOT = "BotFileRoot";

        private static readonly Dictionary<string, BotInfoModel> BotInfo;
        static BotManager()
        {
            BotInfo = new Dictionary<string, BotInfoModel>();
            foreach (string filePath in Directory.GetFiles(BOT_INFO_FOLDER, $"*{BOT_INFO_PARAMNAME}", SearchOption.TopDirectoryOnly))
            {
                BotInfoModel botInfoModel = new BotInfoModel().Decode(File.ReadAllText(filePath));
                botInfoModel.IsRunning = false;
                BotInfo.Add(botInfoModel.BotID, botInfoModel);
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

        public void AddBot(string botInfo, ZipArchive botFile)
        {
            // 生成ID
            string botID = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            // 创建存放文件夹
            DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.Combine(BOT_FILE_ROOT, botID));
            // 讲压缩文件解压
            botFile.ExtractToDirectory(directoryInfo.FullName, true);
            // 找可执行文件
            FileInfo fileInfo = directoryInfo.GetFiles("*.exe", SearchOption.AllDirectories).Single();

            BotInfoModel model = new BotInfoModel()
            {
                BotID = botID,
                BotInfo = botInfo,
                BotFilePath = directoryInfo.FullName,
                EXEPath = fileInfo.FullName,
            };
            BotInfo.Add(botID, model);
            SaveBotInfo();
        }

        public List<(string botID, string botInfo)> ListBotInfos()
        {
            return BotInfo.Select(x => (x.Value.BotID, x.Value.BotInfo)).ToList();
        }

        public bool RemoveBot(string botID)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfoModel botInfo = BotInfo[botID];

            StopBot(botInfo.BotID);
            Directory.Delete(botInfo.BotFilePath, true);

            BotInfo.Remove(botID);
            SaveBotInfo();

            return true;
        }

        public bool StartBot(string botID)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfoModel botInfo = BotInfo[botID];
            if (processManager.IsRunning(botInfo.BotID))
            {
                return true;
            }
            botInfo.IsRunning = true;
            return processManager.Start(botID, botInfo.EXEPath);
        }

        public bool StopBot(string botID)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfoModel botInfo = BotInfo[botID];
            if (!processManager.IsRunning(botInfo.BotID))
            {
                return true;
            }
            botInfo.IsRunning = false;
            return processManager.Stop(botID);
        }

        public bool UpdateBot(string botID, ZipArchive botFile)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfoModel botInfo = BotInfo[botID];
            
            StopBot(botInfo.BotID);
            Directory.Delete(botInfo.BotFilePath, true);

            DirectoryInfo directoryInfo = Directory.CreateDirectory(botInfo.BotFilePath);
            botFile.ExtractToDirectory(directoryInfo.FullName, true);

            return true;
        }

        public bool UpdateBotInfo(string botID, string botInfo)
        {
            if (!BotInfo.ContainsKey(botID))
                return false;

            BotInfo[botID].BotInfo = botInfo;

            SaveBotInfo();

            return true;
        }

        private static void SaveBotInfo()
        {
            BotInfo.Select(x => x.Value).ToList().ForEach(model =>
            {
                string filePath = Path.Combine(BOT_INFO_FOLDER, $"{model.BotID}{BOT_INFO_PARAMNAME}");
                File.WriteAllText(filePath, model.Encrypt(), Encoding.UTF8);
            });
        }

        public void Dispose()
        {
            SaveBotInfo();
        }
    }

    public class BotInfoModel : EncryptDecodeBase<BotInfoModel>
    {
        public string BotID { get; set; }
        public string BotInfo { get; set; }

        public string BotFilePath { get; set; }

        public string EXEPath { get; set; }

        public bool IsRunning { get; set; }
    }
}
