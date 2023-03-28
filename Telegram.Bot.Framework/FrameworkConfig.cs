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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Session;
using Telegram.Bot.Framework.Logger;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.MiddlewarePipelines;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.InternalManagers;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal class FrameworkConfig : IConfig
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISession, InternalSession>();
            // 设置Log
            services.AddLogger(model => 
            {
#if DEBUG
                model.LogLevel = LogType.Debug;
#else
                model.LogLevel = LogType.Warning;
#endif
                model.EnableConsoleLog = true;
                model.EnableFileLog = true;
                model.LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "Telegram.Bot.Framework.log");
            });

            services.AddScoped<IParamManager, MyParamManager>();

            foreach (Type item in ObjectHelper.GetSameType(typeof(AbstractMiddlewarePipeline)))
            {
                services.AddSingleton(typeof(AbstractMiddlewarePipeline), item);
            };
        }
    }
}
