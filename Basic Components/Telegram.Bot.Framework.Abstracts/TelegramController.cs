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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts;

/// <summary>
/// Telegram 控制器
/// </summary>
/// <remarks>
/// 控制器，继承控制器类即可
/// </remarks>
public abstract class TelegramController
{
    /// <summary>
    /// 
    /// </summary>
    protected TelegramUserChatContext Chat { get; private set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    protected ILogger Logger { get; private set; } = null!;

    /// <summary>
    /// 控制器执行
    /// </summary>
    /// <param name="Chat"></param>
    /// <param name="func"></param>
    /// <param name="controllerParamManager"></param>
    /// <returns></returns>
    public virtual async Task ControllerInvokeAsync(TelegramUserChatContext Chat, Func<object, object[], Task> func, IControllerParamManager controllerParamManager)
    {
        this.Chat = Chat;
        Logger = this.Chat.UserScopeService.GetRequiredService<ILogger<TelegramController>>();
        try
        {
            await func(this, controllerParamManager.GetParams() ?? []);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 获取消息创建器
    /// </summary>
    /// <returns></returns>
    protected IMessageBuilder? GetMessageBuilder() => Chat.UserScopeService.GetService<IMessageBuilder>();

    /// <summary>
    /// 发送文本消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected async Task<Message> SendMessage(string message) => await Chat!.BotClient
            .SendTextMessageAsync(Chat.UserChatID, message, parseMode: Types.Enums.ParseMode.Html);

    /// <summary>
    /// 发送附带图片组的消息
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="message"></param>
    /// <param name="imagePaths"></param>
    /// <returns></returns>
    protected async Task<Message[]?> SendMediaGroup(IMessageBuilder message, string[] imagePathOrID)
    {
        if (imagePathOrID == null || imagePathOrID.Length == 0)
            return null!;

        // 初始化
        var images = imagePathOrID
            .Select(PathOrID =>
            {
                InputMediaPhoto inputMediaPhoto;
                if (!System.IO.File.Exists(PathOrID) && !Path.HasExtension(PathOrID))
                    // 文件不存在且没有扩展名，视作文件ID
                    inputMediaPhoto = new(new InputFileId(PathOrID));
                else
                    // 否则，是为文件
                    inputMediaPhoto = new(new InputFileStream(new FileStream(PathOrID, FileMode.Open), Path.GetFileName(PathOrID)));
                return inputMediaPhoto;
            })
            .ToList();

        // 创建HTML消息
        var htmlMessage = message.Build();
        // 进行设定
        if (!string.IsNullOrEmpty(htmlMessage))
        {
            images[0].Caption = htmlMessage;
            images[0].ParseMode = Types.Enums.ParseMode.Html;
        }

        try
        {
            // 发送
            return await Chat!.BotClient.SendMediaGroupAsync(Chat.UserChatID,
                images);
        }
        catch (Exception)
        {
            return null!;
        }
        finally
        {
            // 关闭文件流
            foreach (var item in images)
            {
                if (item.Media is not InputFileStream inputFileStream)
                    continue;

                var stream = inputFileStream?.Content;
                stream?.Dispose();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="PathOrID"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    protected async Task<Message?> SendFile(string message, string PathOrID, string fileName = "")
    {
        if (string.IsNullOrEmpty(PathOrID))
            return null!;

        InputFile inputFile;
        if (!System.IO.File.Exists(PathOrID))
        {
            inputFile = new InputFileId(PathOrID);
        }
        else
        {
            var name = string.IsNullOrEmpty(fileName)
                ? Path.GetFileName(PathOrID)
                : string.IsNullOrEmpty(Path.GetExtension(fileName)) ?
                    $"{fileName}{Path.GetExtension(PathOrID)}" : fileName;
            inputFile = new InputFileStream(new FileStream(PathOrID, FileMode.Open), name);
        }

        try
        {
            return await Chat.BotClient.SendDocumentAsync(Chat.UserChatID, inputFile,
                caption: message, parseMode: Types.Enums.ParseMode.Html);
        }
        catch (Exception)
        {
            return null!;
        }
        finally
        {
            if (inputFile is InputFileStream stream)
                stream?.Content?.Dispose();
        }
    }
}
