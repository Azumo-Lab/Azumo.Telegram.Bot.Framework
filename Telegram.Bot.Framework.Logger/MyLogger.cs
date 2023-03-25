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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.Logger
{
    /// <summary>
    /// 
    /// </summary>
    internal class MyLogger : ILogger
    {
        private static LoggerSettingModel LoggerSettingModel = null!;

        private static StreamWriter LogWrite = null!;

        public MyLogger(IServiceProvider serviceProvider)
        {
            LoggerSettingModel loggerSettingModel = serviceProvider.GetService<LoggerSettingModel>()!;
            if (!loggerSettingModel.IsNull())
                LoggerSettingModel = loggerSettingModel;

            if (LoggerSettingModel.EnableFileLog)
            {
                if (!LogWrite.IsNull())
                    return;

                // Log文件路径是空的
                if (LoggerSettingModel.LogFilePath.IsEmpty())
                    LoggerSettingModel.LogFilePath = Path.GetFileName($"{AppDomain.CurrentDomain.FriendlyName}.log");

                FileStream fileStream;

                string dir = Path.GetDirectoryName(LoggerSettingModel.LogFilePath);
                if (!dir.IsEmpty() && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (!File.Exists(LoggerSettingModel.LogFilePath))
                    fileStream = File.Create(LoggerSettingModel.LogFilePath);
                else
                    fileStream = File.Open(LoggerSettingModel.LogFilePath, FileMode.Append);

                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                LogWrite = new StreamWriter(fileStream, Encoding.UTF8);
                LogWrite.AutoFlush = true;
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (!LogWrite.IsNull())
                LogWrite.Dispose();
        }

        public void DebugLog(string message)
        {
            Log(message, LogType.Debug);
        }

        public void ErrorLog(string message)
        {
            Log(message, LogType.Error);
        }

        public void InformationLog(string message)
        {
            Log(message, LogType.Information);
        }

        public void Log(string message, LogType logType)
        {
            if (message.IsEmpty())
                return;

            if (LoggerSettingModel.LogLevel > logType)
                return;

            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{logType}][{message}]";
            
            switch (logType)
            {
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintLog(logMessage);
                    Console.ResetColor();
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    PrintLog(logMessage);
                    Console.ResetColor();
                    break;
                case LogType.Information:
                    PrintLog(logMessage);
                    break;
                case LogType.Debug:
                    PrintLog(logMessage);
                    break;
                default:
                    break;
            }
        }

        private void PrintLog(string message)
        {
            if (LoggerSettingModel.EnableConsoleLog)
            {
                Console.WriteLine(message);
            }
            if (LoggerSettingModel.EnableFileLog)
            {
                LogWrite.WriteLine(message);
            }
        }

        public void WarningLog(string message)
        {
            Log(message, LogType.Warning);
        }
    }
}
