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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pipeline.Framework.Abstracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPipelineBuilder<T>
    {
        /// <summary>
        /// 添加工序
        /// </summary>
        /// <param name="procedure"></param>
        public IPipelineBuilder<T> AddProcedure(IProcedure<T> procedure);

        /// <summary>
        /// 将工序组装成流水线
        /// </summary>
        public IPipelineBuilder<T> CreatePipeline(string pipelineName);

        /// <summary>
        /// 创建流水线控制器
        /// </summary>
        /// <returns></returns>
        public IPipelineController<T> BuilderPipelineController();
    }
}
