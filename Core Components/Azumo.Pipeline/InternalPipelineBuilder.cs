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

namespace Azumo.Pipeline
{
    /// <summary>
    /// 内部实现的流水线创建类
    /// </summary>
    internal class InternalPipelineBuilder<T> : IPipelineBuilder<T>
    {
        private readonly List<IProcessAsync<T>> __Procedures = new();
        private readonly IPipelineController<T> __Controller;
        private readonly List<Type>? __TypeList;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public InternalPipelineBuilder(List<Type>? TypeList = null)
        {
            __Controller = PipelineFactory.CreateIPipelineController<T>();
            __TypeList = TypeList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedure"></param>
        /// <returns></returns>
        public IPipelineBuilder<T> AddProcedure(IProcessAsync<T> procedure)
        {
            __Procedures.Add(procedure);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedure"></param>
        /// <returns></returns>
        public IPipelineBuilder<T> AddProcedure(string procedure)
        {
            if (__TypeList == null)
                return this;
            Type? type = __TypeList!.Where(x => x.Name == procedure).FirstOrDefault();
            if (type == null)
                return this;

            return Activator.CreateInstance(type!) is not IProcessAsync<T> processAsync ? this : AddProcedure(processAsync);
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
