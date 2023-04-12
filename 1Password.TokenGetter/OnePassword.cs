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

namespace _1Password.TokenGetter
{
    /// <summary>
    /// 
    /// </summary>
    public class OnePassword : IDisposable
    {
        private static string OnePasswordPath = string.Empty;
        private readonly Process CmdProcess = new Process();
        public static void Download()
        {

        }

        public static void Install()
        {

        }

        public static void SetPath(string path)
        {
            OnePasswordPath = path;
        }

        public void Dispose()
        {
            CmdProcess.Kill();
            CmdProcess.Dispose();
        }

        public string Get(string path)
        {
            string command = $"\"{OnePasswordPath}\" read {path}";
            CmdProcess.StartInfo = new ProcessStartInfo
            {
                StandardErrorEncoding = System.Text.Encoding.UTF8,
                StandardInputEncoding = System.Text.Encoding.UTF8,
                StandardOutputEncoding = System.Text.Encoding.UTF8,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                FileName = "cmd.exe",
                WindowStyle = ProcessWindowStyle.Normal,
                CreateNoWindow = false,
            };
            CmdProcess.Start();
            CmdProcess.StandardInput.WriteLine(command);
            CmdProcess.WaitForExit();
            string line;
            List<string> lines = new List<string>();
            while ((line = CmdProcess.StandardOutput.ReadLine()) != null)
            {
                lines.Add(line);
            }
            lines = lines.Skip(2).Where(x => !x.StartsWith("C:\\")).Where(x => !string.IsNullOrEmpty(x)).ToList();
            return lines.FirstOrDefault();
        }
    }
}
