﻿//  <Telegram.Bot.Framework>
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

using Azumo.Pipeline;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.CorePipelines;

[DependencyInjection(ServiceLifetime.Singleton, typeof(ITelegramService))]
internal class PipelineService : ITelegramService
{
    public void AddServices(IServiceCollection services) =>
        services.AddScoped(x => PipelineFactory.CreateIPipelineBuilder<TelegramUserChatContext>()

        .AddProcedure(new PipelineControllerFilter())
        .AddProcedure(new PipelineControllerParams())
        .AddProcedure(new PipelineControllerInvoker())
        .AddProcedure(new PipelineClear())

        .CreatePipeline(UpdateType.Message)

        .BuilderPipelineController());
}