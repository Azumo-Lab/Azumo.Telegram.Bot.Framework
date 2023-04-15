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

using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;

namespace Telegram.Bot.Framework.InternalImplementation.Params.ParamMiddlewares
{
    /// <summary>
    /// 
    /// </summary>
    internal class InitParamCatchMiddleware : IParamMiddleware
    {
        private string __Command;
        public async Task<bool> Execute(ITelegramSession Session, IParamManager paramManager, IControllerContext controllerContext, ParamMiddlewareDelegate Next)
        {
            if (controllerContext.ParamModels.Count == 0)
                return ReturnTrue(paramManager);

            string command;
            if (__Command != (command = controllerContext.BotCommandAttribute.Command))
                SetCommand(command, paramManager);

            return await Next(Session, paramManager, controllerContext);
        }

        private static bool ReturnTrue(IParamManager paramManager)
        {
            paramManager.Clear();
            return true;
        }

        private void SetCommand(string command, IParamManager paramManager)
        {
            __Command = command;
            paramManager.Clear();
        }
    }
}
