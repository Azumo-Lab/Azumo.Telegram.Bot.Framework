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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Telegram.Bot.Upgrader.Bot
{
    /// <summary>
    /// 
    /// </summary>
    internal class ProcessManager : IProcessManager, IDisposable
    {
        private readonly Dictionary<string, Process> ProcessMap = new Dictionary<string, Process>();

        public ProcessManager()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            Dispose();
        }

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
        }

        public bool IsRunning(string ID)
        {
            return ProcessMap.ContainsKey(ID);
        }

        public bool Start(string ID, string EXEPath)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = EXEPath,
                WorkingDirectory = Path.GetDirectoryName(EXEPath),
            };

            ProcessMap.TryAdd(ID, process);

            ProcessMap[ID].Start();

            return true;
        }

        public bool Stop(string ID)
        {
            if (!ProcessMap.ContainsKey(ID))
                return true;

            Process process = ProcessMap[ID];
            if (!process.HasExited)
                process.Kill();
            process.Close();
            process.Dispose();
            ProcessMap.Remove(ID);

            return true;
        }
    }
}
