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

using System.Diagnostics;

namespace Telegram.Bot.Upgrader.Bot
{
    /// <summary>
    /// 执行管理
    /// </summary>
    internal class ProcessManager : IProcessManager, IDisposable
    {
        private readonly Dictionary<string, Process> ProcessMap = new();

        public ProcessManager()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        /// <summary>
        /// 销毁时候的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// 销毁，结束所有程序
        /// </summary>
        public void Dispose()
        {
            foreach (KeyValuePair<string, Process> item in ProcessMap)
            {
                Process process = item.Value;
                if (!process.HasExited)
                {
                    process.Kill();
                }
                process.Close();
                process.Dispose();
            }
            ProcessMap.Clear();
        }

        /// <summary>
        /// 判断是否正在执行
        /// </summary>
        /// <param name="ID">BotID</param>
        /// <returns>True正在执行／False已停止</returns>
        public bool IsRunning(string ID)
        {
            return ProcessMap.ContainsKey(ID);
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="ID">BotID</param>
        /// <param name="EXEPath">可执行文件的路径</param>
        /// <returns></returns>
        public bool Start(string ID, string EXEPath)
        {
            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = EXEPath,
                    WorkingDirectory = Path.GetDirectoryName(EXEPath),
                }
            };

            _ = ProcessMap.TryAdd(ID, process);

            _ = ProcessMap[ID].Start();

            return true;
        }

        /// <summary>
        /// 停止执行
        /// </summary>
        /// <param name="ID">BotID</param>
        /// <returns>是否成功</returns>
        public bool Stop(string ID)
        {
            if (!ProcessMap.ContainsKey(ID))
                return true;

            Process process = ProcessMap[ID];
            if (!process.HasExited)
                process.Kill();
            process.Close();
            process.Dispose();
            _ = ProcessMap.Remove(ID);

            return true;
        }
    }
}
