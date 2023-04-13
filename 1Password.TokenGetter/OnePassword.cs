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

using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace _1Password.TokenGetter
{
    /// <summary>
    /// 
    /// </summary>
    public class OnePasswordCLI : IDisposable
    {
        private static Setting OnePasswordSetting;
        private readonly Process OnePasswordProcess = new Process();

        private class Setting
        {
            public string OnePasswordPath { get; set; } = string.Empty;

            public string Version { get; set; } = string.Empty;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        static OnePasswordCLI()
        {
            OnePasswordSetting = File.Exists(nameof(OnePasswordCLI))
                ? JsonConvert.DeserializeObject<Setting>(File.ReadAllText(nameof(OnePasswordCLI)))!
                : new Setting();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public OnePasswordCLI()
        {
            if (!File.Exists(OnePasswordSetting.OnePasswordPath))
            {
                string msg =
@"The 1Password Cli Could't Be Found, Please Use 'OnePasswordCLI.SetPath(""You 1Password Cli Path"")' To Set The Path
无法找到 1Password Cli 程序的，请使用 'OnePasswordCLI.SetPath(""1Password Cli 程序路径"")' 来设定
";
                throw new FileNotFoundException(msg);
            }
            OnePasswordProcess.StartInfo = new ProcessStartInfo
            {
                StandardErrorEncoding = Encoding.UTF8,
                StandardInputEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                FileName = $"{OnePasswordSetting.OnePasswordPath}",
                WindowStyle = ProcessWindowStyle.Normal,
                CreateNoWindow = false,
                UseShellExecute = false,
            };
        }

        /// <summary>
        /// 下载最新版本
        /// </summary>
        /// <param name="system">系统版本，默认下载Windows版本</param>
        /// <exception cref="Exception"></exception>
        public static void Download(SystemEnum system = SystemEnum.Windows_AMD64)
        {
            if (File.Exists(OnePasswordSetting.OnePasswordPath))
                return;

            string html;
            WebRequest webRequest = WebRequest.Create("https://app-updates.agilebits.com/product_history/CLI2");
            WebResponse webResponse = webRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                html = streamReader.ReadToEnd();
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            List<HtmlNode> list = doc.DocumentNode.SelectNodes("//article")
                .Where(x => !string.IsNullOrEmpty(x.GetAttributeValue("id", string.Empty)))
                .Where(x => string.IsNullOrEmpty(x.GetAttributeValue("class", string.Empty)))
                .OrderByDescending(x => x.GetAttributeValue("id", string.Empty))
                .ToList();

            HtmlNode htmlNode = list.FirstOrDefault();
            string version = htmlNode.GetAttributeValue("id", "v00");
            if (htmlNode == null || (version == OnePasswordSetting.Version))
                return;

            List<HtmlNode> htmlNodes = htmlNode.SelectNodes($"{htmlNode.XPath}/div[@class=\"cli-archs\"]").SelectMany(x => x.SelectNodes($"{x.XPath}/p").ToList()).ToList();

            string systemClass;
            switch (system)
            {
                case SystemEnum.Windows_386:
                case SystemEnum.Windows_AMD64:
                    systemClass = "system windows";
                    break;
                case SystemEnum.Apple_Universal:
                    systemClass = "system apple";
                    break;
                case SystemEnum.Linux_386:
                case SystemEnum.Linux_AMD64:
                case SystemEnum.Linux_ARM:
                case SystemEnum.Linux_ARM64:
                    systemClass = "system linux";
                    break;
                case SystemEnum.FreeBSD_386:
                case SystemEnum.FreeBSD_AMD64:
                case SystemEnum.FreeBSD_ARM:
                case SystemEnum.FreeBSD_ARM64:
                    systemClass = "system freebsd";
                    break;
                case SystemEnum.OpenBSD_386:
                case SystemEnum.OpenBSD_AMD64:
                case SystemEnum.OpenBSD_ARM64:
                    systemClass = "system openbsd";
                    break;
                default:
                    throw new Exception();
            }
            HtmlNode downloadUrlNode = htmlNodes.Where(x => x.GetAttributeValue("class", "") == systemClass).FirstOrDefault();
            string? downloadURL = downloadUrlNode.SelectNodes($"{downloadUrlNode.XPath}/a[@title=\"Download for {system.ToString().Split('_', 2, StringSplitOptions.None)[1].ToLower()}\"]")
                .FirstOrDefault()?.GetAttributeValue("href", string.Empty);

            if (string.IsNullOrEmpty(downloadURL))
                return;

            if (!File.Exists(Path.GetFileName(downloadURL)))
            {
                WebRequest downloadRequest = WebRequest.Create(downloadURL);
                WebResponse downloadResponse = downloadRequest.GetResponse();
                using (FileStream fs = new FileStream(Path.GetFileName(downloadURL), FileMode.OpenOrCreate))
                {
                    downloadResponse.GetResponseStream().CopyTo(fs);
                }
                downloadResponse.Close();
                OnePasswordSetting.Version = version;
            }

            // 解压文件
            DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.GetFileNameWithoutExtension(downloadURL));
            ZipFile.ExtractToDirectory(Path.GetFileName(downloadURL), directoryInfo.FullName, true);
            try
            {
                FileInfo exeFile = directoryInfo.GetFiles("*.exe", SearchOption.TopDirectoryOnly).First();

                string opPath = exeFile.FullName;
                SetPath(opPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Install()
        {
            try
            {
                string programsPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                DirectoryInfo OnePasswordCliPath = Directory.CreateDirectory(Path.Combine(programsPath, "1Password Cli"));

                FileInfo fileInfo = new FileInfo(OnePasswordSetting.OnePasswordPath);
                fileInfo.MoveTo(Path.Combine(OnePasswordCliPath.FullName, fileInfo.Name));

                
            }
            catch (UnauthorizedAccessException ex)
            {
                string msg =
@"Insufficient permissions, please run with administrator permissions
权限不足，请使用管理员权限运行
";
                throw new UnauthorizedAccessException(msg, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 设置可执行文件路径
        /// </summary>
        /// <param name="path">路径</param>
        public static void SetPath(string path)
        {
            OnePasswordSetting.OnePasswordPath = path;
            File.WriteAllText(nameof(OnePasswordCLI), JsonConvert.SerializeObject(OnePasswordSetting));
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            OnePasswordProcess.Kill();
            OnePasswordProcess.Dispose();
        }

        /// <summary>
        /// 读取指定字段， 一个路径的格式：
        /// op://保险库名称/项目名称/字段名称
        /// </summary>
        /// <param name="path">以"op://"开头的路径地址</param>
        /// <returns>指定路径字段的内容</returns>
        public string Read(string path)
        {
            return GetResult($" read {path}").FirstOrDefault();
        }

        /// <summary>
        /// 将文件中的OP://数据替换为机密数据
        /// </summary>
        /// <param name="inputFile">输入文件</param>
        /// <param name="outPutFile">输出文件</param>
        public void Inject(string inputFile, string outPutFile)
        {
            if (!File.Exists(inputFile))
                return;
            if (!Directory.Exists(Path.GetDirectoryName(outPutFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(outPutFile));

            bool fileNameChange = false;
            string souceFileName = string.Empty;
            string tplFileName = string.Empty;
            if (Path.GetExtension(inputFile).ToLower() != ".tpl")
            {
                File.Move(souceFileName = inputFile, tplFileName = Path.Combine(Path.GetDirectoryName(inputFile), $"{souceFileName}.tpl"));
                fileNameChange = true;
            }

            GetResult($" inject -i \"{tplFileName}\" -o \"{outPutFile}\"");

            if (fileNameChange)
                File.Move(tplFileName, souceFileName);
        }

        /// <summary>
        /// 将文件中的OP://数据替换为机密数据
        /// </summary>
        /// <param name="inputFile">输入文件</param>
        /// <param name="outPutFile">输出文件</param>
        public void Inject(string file)
        {
            if (!File.Exists(file))
                return;

            string itemFile = $"{Path.GetFileName(file)}.item";

            GetResult($" inject -i \"{file}\" -o \"{Path.Combine(Path.GetDirectoryName(file), itemFile)}\"");

            File.Delete(file);
            File.Move(Path.Combine(Path.GetDirectoryName(file), itemFile), file);
        }

        /// <summary>
        /// 获取指定UUID项目的内容
        /// </summary>
        /// <param name="uuid">指定的UUID</param>
        /// <returns>项目内容</returns>
        public T ItemGet<T>(string uuid) where T : class
        {
            string json = ItemGet(uuid, FormatEnum.JSON);
            return JsonConvert.DeserializeObject<T>(json)!;
        }

        /// <summary>
        /// 获取指定UUID项目的内容
        /// </summary>
        /// <param name="uuid">指定的UUID</param>
        /// <returns>项目内容</returns>
        public string ItemGet(string uuid, FormatEnum formatEnum = FormatEnum.NONE)
        {
            string command = $" item get {uuid}";
            switch (formatEnum)
            {
                case FormatEnum.NONE:
                    break;
                case FormatEnum.JSON:
                    command += " --format json";
                    break;
                default:
                    break;
            }
            return string.Join(Environment.NewLine, GetResult(command));
        }

        /// <summary>
        /// 执行1Password
        /// </summary>
        /// <param name="command">执行内容</param>
        /// <returns>返回内容</returns>
        private List<string> GetResult(string command)
        {
            OnePasswordProcess.StartInfo.Arguments = command;
            OnePasswordProcess.Start();
            OnePasswordProcess.WaitForExit();
            string line;
            List<string> lines = new List<string>();
            while ((line = OnePasswordProcess.StandardOutput.ReadLine()) != null)
            {
                lines.Add(line);
            }
            lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToList();
            return lines;
        }

        private void InputResult(string command)
        {
            OnePasswordProcess.StartInfo.Arguments = command;
            OnePasswordProcess.Start();
            while (true)
            {
                if (OnePasswordProcess.HasExited)
                    return;

                string line;
                if((line = OnePasswordProcess.StandardOutput.ReadLine()) != null)
                    Console.WriteLine(line);

                if (!OnePasswordProcess.WaitForInputIdle(100))
                {
                    ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                    if (consoleKeyInfo.Key == ConsoleKey.Enter)
                    {
                        OnePasswordProcess.StandardInput.WriteLine();
                    }
                    else
                    {
                        OnePasswordProcess.StandardInput.Write(consoleKeyInfo.KeyChar);
                    }
                }
            }
        }
    }
}
