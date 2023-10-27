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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipelines
{
    internal class ProcessControllerAuthenticate : IProcessAsync<(TGChat tGChat, IControllerContext controllerContext)>, IPipelineName
    {
        public string Name => "ProcessControllerAuthenticate";

        public async Task<(TGChat tGChat, IControllerContext controllerContext)> ExecuteAsync((TGChat tGChat, IControllerContext controllerContext) t, IPipelineController<(TGChat tGChat, IControllerContext controllerContext)> pipelineController)
        {
            List<AuthenticateAttribute> attributes = new();
            if (t.controllerContext?.BotCommand?.MethodAttributes != null)
                attributes.AddRange(t.controllerContext.BotCommand.MethodAttributes.Where(x => x is AuthenticateAttribute).Select(x => (AuthenticateAttribute)x));
            if (t.controllerContext?.BotCommand?.ControllerAttributes != null)
                attributes.AddRange(t.controllerContext.BotCommand.ControllerAttributes.Where(x => x is AuthenticateAttribute).Select(x => (AuthenticateAttribute)x));

            if (!attributes.Any())
                return await pipelineController.NextAsync(t);

            foreach (AuthenticateAttribute authenticate in attributes)
                if (t.tGChat.Authenticate.RoleName == null)
                    return await pipelineController.StopAsync(t);
                else
                    foreach (string item in authenticate.RoleName)
                        if (t.tGChat.Authenticate.RoleName.Contains(item))
                            return await pipelineController.NextAsync(t);

            return await pipelineController.StopAsync(t);
        }
    }
}
