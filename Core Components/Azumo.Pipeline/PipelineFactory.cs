//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Azumo.Pipeline
{
    /// <summary>
    /// 流水线创建工厂
    /// </summary>
    public static class PipelineFactory
    {
        /// <summary>
        /// 内部实现
        /// </summary>
        internal static IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        static PipelineFactory()
        {
            ServiceCollection serviceDescriptors = new();

            _ = serviceDescriptors.AddSingleton<IPipelineFilter, InternalPipelineFilter>();

            ServiceAction(serviceDescriptors);

            ServiceProvider = serviceDescriptors.BuildServiceProvider();
        }

        /// <summary>
        /// 
        /// </summary>
        public static Action<IServiceCollection> ServiceAction { get; set; } = (coll) => { };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedures"></param>
        /// <param name="pipelineController"></param>
        /// <returns></returns>
        internal static IPipeline<T> CreateIPipeline<T>(IProcessAsync<T>[] procedures, IPipelineController<T> pipelineController)
        {
            return new InternalPipeline<T>(procedures, pipelineController);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static IPipelineController<T> CreateIPipelineController<T>()
        {
            return new InternalPipelineController<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IPipelineBuilder<T> CreateIPipelineBuilder<T>()
        {
            return new InternalPipelineBuilder<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TypeList"></param>
        /// <returns></returns>
        public static IPipelineBuilder<T> CreateIPipelineBuilder<T>(List<Type> TypeList)
        {
            return new InternalPipelineBuilder<T>(TypeList);
        }
    }
}
