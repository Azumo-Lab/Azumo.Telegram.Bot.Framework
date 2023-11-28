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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection.Metadata.Ecma335;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.InternalInterface;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace Telegram.Bot.Framework.Abstracts
{
    /// <summary>
    /// 控制器
    /// </summary>
    public abstract class TelegramController
    {
        /// <summary>
        /// 
        /// </summary>
        protected TGChat Chat { get; private set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        protected ILogger Logger { get; private set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Chat"></param>
        /// <param name="func"></param>
        /// <param name="controllerParamManager"></param>
        /// <returns></returns>
        internal async Task ControllerInvokeAsync(TGChat Chat, Func<TelegramController, object[], Task> func, IControllerParamManager controllerParamManager)
        {
            this.Chat = Chat;
            Logger = this.Chat.UserService.GetRequiredService<ILogger<TelegramController>>();
            try
            {
                if (func(this, controllerParamManager.GetParams() ?? Array.Empty<object>()) is Task task)
                    await task;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                controllerParamManager.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IMessageBuilder MessageBuilder()
        {
            return Chat.UserService.GetService<IMessageBuilder>()!;
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected async Task<Message> SendMessage(string message)
        {
            return await Chat!.BotClient.SendTextMessageAsync(Chat.ChatId, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="imagePaths"></param>
        /// <returns></returns>
        protected async Task<Message[]> SendMediaGroup(string message, string[] imagePaths)
        {
            // 初始化
            List<InputMediaPhoto> image = imagePaths.Select(path =>
            {
                InputMediaPhoto inputMediaPhoto = new(new InputFileStream(new FileStream(path, FileMode.Open), Path.GetFileName(path)));
                return inputMediaPhoto;
            }).ToList();

            // 进行设定
            image[0].Caption = message;
            image[0].ParseMode = Types.Enums.ParseMode.Html;

            // 发送
            return await Chat!.BotClient.SendMediaGroupAsync(Chat.ChatId,
                image);
        }
    }
}
