//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Linq;
using Telegram.Bot.Framework.PipelineMiddleware;

namespace Telegram.Bot.Framework.InternalCore.PipelineMiddleware
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    internal class PipelineController<TInput, TResult> : IPipelineController<TInput, TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipelines"></param>
        public PipelineController(Dictionary<object, IPipeline<TInput, TResult>> pipelines) =>
            _Dic = new Dictionary<object, IPipeline<TInput, TResult>>(pipelines);

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<object, IPipeline<TInput, TResult>> _Dic;

        /// <summary>
        /// 
        /// </summary>
        private object? _CurrentKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IPipeline<TInput, TResult> this[object key]
        {
            get
            {
                if (!_Dic.TryGetValue(key, out var pipeline))
                    throw new Exception();

                _CurrentKey = key;

                return pipeline;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IPipeline<TInput, TResult> CurrentPipeline
        {
            get
            {
                if (_CurrentKey != null)
                    return this[_CurrentKey];
                else if (_Dic.Count == 1)
                    return _Dic.First().Value;
                throw new Exception();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pipeline"></param>
        public void Add(object key, IPipeline<TInput, TResult> pipeline) =>
            _Dic.TryAdd(key, pipeline);

        /// <summary>
        /// 
        /// </summary>
        public void Clear() =>
            _Dic.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key) =>
            _Dic.Remove(key);
    }
}
