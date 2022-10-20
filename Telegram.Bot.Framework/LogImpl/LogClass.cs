//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using Telegram.Bot.Framework.Abstract;

namespace Telegram.Bot.Framework.LogImpl
{
    /// <summary>
    /// Log的实现类
    /// </summary>
    public class LogClass : ILog
    {
        private static StreamWriter LogWriter;
        private static Timer LogTimer = new Timer();
        private static DateTime Now;

        private IServiceProvider serviceProvider;
        static LogClass()
        {
            Now = DateTime.Now;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            LogTimer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            LogTimer.Elapsed += new ElapsedEventHandler((obj, e) =>
            {

            });
            LogTimer.Start();

            LogWriter ??= new StreamWriter(new FileStream($"{Now:yyyy-MM-dd}.log", FileMode.OpenOrCreate));
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            LogWriter?.Dispose();
        }

        public LogClass(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Log(string message)
        {
            throw new NotImplementedException();
        }
    }
}
