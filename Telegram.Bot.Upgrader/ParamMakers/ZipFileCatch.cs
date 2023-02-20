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
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract;

namespace Telegram.Bot.Upgrader.ParamMakers
{
    /// <summary>
    /// 
    /// </summary>
    internal class ZipFileCatch : IParamMaker
    {
        private ZipArchive ZipArchive = null!;

        public async Task<object> GetParam(TelegramContext context, IServiceProvider serviceProvider)
        {
            return await Task.FromResult(ZipArchive);
        }

        public async Task<bool> ParamCheck(TelegramContext context, IServiceProvider serviceProvider)
        {
            async Task<bool> IsNotZipFile()
            {
                await context.SendTextMessage("你发送的不是Zip文件，或者文件已损坏，请重新打包发送");
                return false;
            }

            if (context.Update.Type != Types.Enums.UpdateType.Message
                || context.Update.Message?.Type != Types.Enums.MessageType.Document
                || context.Update.Message.Document?.FileId == null)
                return await IsNotZipFile();

            MemoryStream memoryStream = new();
            await context.BotClient.GetInfoAndDownloadFileAsync(context.Update.Message.Document.FileId, memoryStream);

            try
            {
                ZipArchive = new ZipArchive(memoryStream);

                return true;
            }
            catch (Exception)
            { }

            return await IsNotZipFile();
        }
    }
}
