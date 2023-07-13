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

using Pipeline.Framework.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pipeline.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal class PipelineController<T> : IPipelineController<T>
    {
        private readonly Dictionary<string, IPipeline<T>> __Pipelines = new();

        private PipelineDelegate<T>? __Next;
        private IPipeline<T>? __NowPipeline;

        public void AddPipeline(string pipelineName, IPipeline<T> pipeline)
        {
            __Pipelines.Add(pipelineName, pipeline);
        }

        public void ChangePipeline(string pipelineName)
        {
            if (__Pipelines.TryGetValue(pipelineName, out IPipeline<T>? val))
                __NowPipeline = val;
        }

        public async Task<T> Next(T t)
        {
            if (__NowPipeline != null)
                return await __NowPipeline.Invoke(t);
            else
                return await __Next(t, this);
        }

        public Task<T> Stop(T t)
        {
            return Task.FromResult(t);
        }

        void IPipelineController<T>.SetNext(PipelineDelegate<T> pipelineDelegate)
        {
            __Next = pipelineDelegate;
            __NowPipeline = null!;
        }
    }
}
