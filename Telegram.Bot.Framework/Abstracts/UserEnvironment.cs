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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Pipeline.Abstracts;
using Telegram.Bot.Framework.Pipelines;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts
{
    internal class UserEnvironment
    {
        public static void Add(IServiceCollection services)
        {
            services.AddScoped(x =>
            {
                return PipelineFactory.CreateIPipelineBuilder<TGChat>()
                .AddProcedure(new ProcessControllerInvoke())
                .CreatePipeline(UpdateType.Message)
                .BuilderPipelineController();
            });
        }

        public async static Task InvokeAsync(TGChat chat)
        {
            IServiceProvider serviceProvider = chat.UserService;

            IPipelineController<TGChat> pipelineController = serviceProvider.GetService<IPipelineController<TGChat>>();
           
            _ = await pipelineController.SwitchTo(chat.Type, chat);
        }
    }
}
