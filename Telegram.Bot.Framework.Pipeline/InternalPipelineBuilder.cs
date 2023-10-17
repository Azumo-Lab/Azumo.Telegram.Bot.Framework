﻿//  <Telegram.Bot.Framework>
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

using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipeline
{
    /// <summary>
    /// 内部实现的流水线创建类
    /// </summary>
    internal class InternalPipelineBuilder<T> : IPipelineBuilder<T>
    {
        private readonly List<IProcess<T>> __Procedures = new();
        private readonly IPipelineController<T> __Controller;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public InternalPipelineBuilder()
        {
            __Controller = PipelineFactory.CreateIPipelineController<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedure"></param>
        /// <returns></returns>
        public IPipelineBuilder<T> AddProcedure(IProcess<T> procedure)
        {
            __Procedures.Add(procedure);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPipelineController<T> BuilderPipelineController()
        {
            return __Controller;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="PipelineNameType"></typeparam>
        /// <param name="pipelineName"></param>
        /// <returns></returns>
        public IPipelineBuilder<T> CreatePipeline<PipelineNameType>(PipelineNameType pipelineName) where PipelineNameType : notnull
        {
            if (pipelineName == null)
                throw new ArgumentNullException(nameof(pipelineName));

            __Controller.AddPipeline(pipelineName, PipelineFactory.CreateIPipeline(__Procedures.ToArray(), __Controller));
            __Procedures.Clear();
            return this;
        }
    }
}