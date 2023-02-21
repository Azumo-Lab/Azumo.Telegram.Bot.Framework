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

namespace Telegram.Bot.Upgrader.Bot
{
    /// <summary>
    /// Bot管理
    /// </summary>
    internal interface IBotManager
    {
        /// <summary>
        /// 添加一个Bot
        /// </summary>
        /// <param name="botInfo">Bot信息</param>
        /// <param name="botFile">Bot的执行文件的压缩文件</param>
        void AddBot(string botInfo, ZipArchive botFile);

        /// <summary>
        /// 删除一个Bot
        /// </summary>
        /// <param name="botID">BotID</param>
        bool RemoveBot(string botID);

        /// <summary>
        /// 升级一个Bot
        /// </summary>
        /// <param name="botID">BotID</param>
        /// <param name="botFile">要升级的可执行文件的压缩文件</param>
        bool UpdateBot(string botID, ZipArchive botFile);

        /// <summary>
        /// 更新一个Bot的信息
        /// </summary>
        /// <param name="botID">BotID</param>
        /// <param name="botInfo">Bot的信息</param>
        bool UpdateBotInfo(string botID, string botInfo);

        /// <summary>
        /// 停止一个Bot
        /// </summary>
        /// <param name="botID">BotID</param>
        bool StopBot(string botID);

        /// <summary>
        /// 启动一个Bot
        /// </summary>
        /// <param name="botID">BotID</param>
        bool StartBot(string botID);

        /// <summary>
        /// 列出Bot的信息
        /// </summary>
        /// <returns></returns>
        List<BotInfoModel> ListBotInfos();
    }
}
