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
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs.Abstract;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs.Analyzes;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs.AttributeAnalyzes;

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs
{
    /// <summary>
    /// 整个框架的解析类
    /// </summary>
    internal class FrameworkAnalyze
    {
        private static readonly ServiceProvider ServiceProvider;
        private static List<CommandInfos> CommandInfos;
        private static object _LockObj = new object();

        /// <summary>
        /// 初始化
        /// </summary>
        static FrameworkAnalyze()
        {
            // 创建服务
            ServiceCollection serviceDescriptors = new ServiceCollection();

            // 添加服务
            List<Type> types = TypesHelper.GetTypes<IAttributeAnalyze>();
            foreach (Type type in types)
                serviceDescriptors.AddScoped(typeof(IAttributeAnalyze), type);

            ServiceProvider = serviceDescriptors.BuildServiceProvider();
        }

        /// <summary>
        /// 解析系统的配置信息等
        /// </summary>
        /// <returns>Command的配置信息</returns>
        internal static List<CommandInfos> Analyze()
        {
            if (CommandInfos != null)
                return CommandInfos;

            lock (_LockObj)
            {
                try
                {
                    // 创建一个Scope，服务范围
                    using (IServiceScope serviceScope = ServiceProvider.CreateScope())
                    {
                        List<CommandInfos> commandInfos = new();
                        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

                        List<Type> controllers = TypesHelper.GetTypes<TelegramController>();
                        foreach (Type controllerType in controllers)
                        {
                            CommandInfos command;
                            ClassAbalyze classAbalyze = new(controllerType);
                            classAbalyze.ServiceProvider = serviceProvider;
                            if ((command = classAbalyze.Analyze(new CommandInfos())) != null)
                                commandInfos.Add(command);
                        }
                        CommandInfos = new List<CommandInfos>(commandInfos);
                        return commandInfos;
                    }
                }
                finally
                {
                    // 销毁
                    ServiceProvider.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取默认相应的Message的Command信息
        /// </summary>
        /// <returns>Command的配置信息</returns>
        internal static List<CommandInfos> GetMessageTypeInfos()
        {
            return Analyze().Where(x => x.MessageType != null).ToList();
        }
    }
}
