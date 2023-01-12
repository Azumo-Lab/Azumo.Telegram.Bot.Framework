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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.Components;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class TelegramContextExtension
    {
        #region 发送消息

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendTextMessage(this TelegramContext context, string message)
        {
            await context.BotClient.SendTextMessageAsync(context.ChatID, message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendTextMessage(this TelegramContext context, TelegramUser telegramUser, string message)
        {
            await context.BotClient.SendTextMessageAsync(telegramUser.ChatID, message);
        }

        /// <summary>
        /// 发送消息，带按钮
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <param name="inlineButtons"></param>
        /// <returns></returns>
        public static async Task SendTextMessage(this TelegramContext context, string message, IEnumerable<InlineButtons> inlineButtons)
        {
            await context.BotClient.SendTextMessageAsync(context.ChatID, message,
                replyMarkup: new InlineKeyboardMarkup(context.CreateInlineKeyboardButton(inlineButtons)));
        }

        /// <summary>
        /// 发送消息，带按钮
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <param name="inlineButtons"></param>
        /// <returns></returns>
        public static async Task SendTextMessage(this TelegramContext context, TelegramUser telegramUser, string message, IEnumerable<InlineButtons> inlineButtons)
        {
            await context.BotClient.SendTextMessageAsync(telegramUser.ChatID, message,
                replyMarkup: new InlineKeyboardMarkup(context.CreateInlineKeyboardButton(inlineButtons)));
        }

        /// <summary>
        /// 回复用户发送的信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task ReplyMessage(this TelegramContext context, string message)
        {
            await context.BotClient.SendTextMessageAsync(context.ChatID, message, 
                replyToMessageId: context.Update.Message.MessageId);
        }

        #endregion

        #region 发送图片

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="PhotoPath"></param>
        /// <returns></returns>
        public static async Task SendPhoto(this TelegramContext context, string PhotoPath)
        {
            string PhotoName = Path.GetFileName(PhotoPath);

            await context.BotClient.SendPhotoAsync(context.ChatID,
                new InputOnlineFile(new FileStream(PhotoPath, FileMode.Open), PhotoName));
        }

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="PhotoPath"></param>
        /// <returns></returns>
        public static async Task SendPhoto(this TelegramContext context, string PhotoPath, string message)
        {
            string PhotoName = Path.GetFileName(PhotoPath);

            await context.BotClient.SendPhotoAsync(context.ChatID,
                new InputOnlineFile(new FileStream(PhotoPath, FileMode.Open), PhotoName), message);
        }

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="Photo"></param>
        /// <returns></returns>
        public static async Task SendPhoto(this TelegramContext context, PhotoSize Photo, string Message)
        {
            await context.BotClient.SendPhotoAsync(context.ChatID, new InputOnlineFile(Photo.FileId), Message);
        }

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="Photo"></param>
        /// <returns></returns>
        public static async Task SendPhoto(this TelegramContext context, PhotoSize Photo)
        {
            await context.BotClient.SendPhotoAsync(context.ChatID, new InputOnlineFile(Photo.FileId));
        }

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="PhotoPath"></param>
        /// <param name="keyboardButton"></param>
        /// <returns></returns>
        public static async Task SendPhoto(this TelegramContext context, string PhotoPath, IEnumerable<InlineButtons> keyboardButton)
        {
            string PhotoName = Path.GetFileName(PhotoPath);

            await context.BotClient.SendPhotoAsync(context.ChatID,
                new InputOnlineFile(new FileStream(PhotoPath, FileMode.Open), PhotoName), 
                replyMarkup: new InlineKeyboardMarkup(CreateInlineKeyboardButton(context, keyboardButton)));
        }

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="Photo"></param>
        /// <param name="keyboardButton"></param>
        /// <returns></returns>
        public static async Task SendPhoto(this TelegramContext context, PhotoSize Photo, IEnumerable<InlineButtons> keyboardButton)
        {
            await context.BotClient.SendPhotoAsync(context.ChatID, 
                new InputOnlineFile(Photo.FileId),
                replyMarkup: new InlineKeyboardMarkup(CreateInlineKeyboardButton(context, keyboardButton)));
        }

        #endregion

        #region 工具
        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="context"></param>
        /// <param name="keyboardButton"></param>
        /// <returns></returns>
        public static IEnumerable<InlineKeyboardButton> CreateInlineKeyboardButton(this TelegramContext context, IEnumerable<InlineButtons> keyboardButton)
        {
            IEnumerable<InlineKeyboardButton> inlineKeyboardButtons = keyboardButton.Select(x => new InlineKeyboardButton(x.Text)
            {
                CallbackData = CreateCallBack(context, x.Callback),
                CallbackGame = x.CallbackGame,
                Url = x.Url,
                LoginUrl = x.LoginUrl,
                Pay = x.Pay,
                SwitchInlineQuery = x.SwitchInlineQuery,
                SwitchInlineQueryCurrentChat = x.SwitchInlineQueryCurrentChat,
                WebApp = x.WebApp,
            }).ToList();

            return inlineKeyboardButtons;
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="context"></param>
        /// <param name="keyboardButton"></param>
        /// <returns></returns>
        public static IEnumerable<InlineKeyboardButton> CreateInlineKeyboardButton(this TelegramContext context, TelegramUser telegramUser, IEnumerable<InlineButtons> keyboardButton)
        {
            IEnumerable<InlineKeyboardButton> inlineKeyboardButtons = keyboardButton.Select(x => new InlineKeyboardButton(x.Text)
            {
                CallbackData = context.CreateCallBack(telegramUser, x.Callback),
                CallbackGame = x.CallbackGame,
                Url = x.Url,
                LoginUrl = x.LoginUrl,
                Pay = x.Pay,
                SwitchInlineQuery = x.SwitchInlineQuery,
                SwitchInlineQueryCurrentChat = x.SwitchInlineQueryCurrentChat,
                WebApp = x.WebApp,
            }).ToList();

            return inlineKeyboardButtons;
        }

        /// <summary>
        /// 创建CallBack
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static string CreateCallBack(this TelegramContext context, Action<TelegramContext> callback)
        {
            ICallBackManager callBackManager = context.UserScope.GetService<ICallBackManager>();
            return callBackManager.CreateCallBack(callback);
        }

        /// <summary>
        /// 创建指定用户的CallBack
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static string CreateCallBack(this TelegramContext context, TelegramUser telegramUser, Action<TelegramContext> callback)
        {
            IUserScopeManager telegramUserScopeManager = context.UserScope.GetService<IUserScopeManager>();
            IServiceScope serviceScope = telegramUserScopeManager.GetUserScope(telegramUser).GetUserServiceScope();

            ICallBackManager callBackManager = serviceScope.ServiceProvider.GetService<ICallBackManager>();
            return callBackManager.CreateCallBack(callback);
        }
        #endregion
    }
}
