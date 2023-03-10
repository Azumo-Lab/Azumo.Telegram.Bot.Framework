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
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract.Params;

namespace Telegram.Bot.Example.Makers
{
    /// <summary>
    /// 
    /// </summary>
    public class MyPhotoParamMaker : IParamMaker
    {
        public async Task<object> GetParam(TelegramContext context, IServiceProvider serviceProvider)
        {
            var photos = context.Update.Message.Photo.ToList();
            return await Task.FromResult(photos.OrderBy(x=>x.FileSize).LastOrDefault());
        }

        public async Task<bool> ParamCheck(TelegramContext context, IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(context.Update.Message?.MediaGroupId))
            {
                return await Task.FromResult(true);
            }
            await context.BotClient.SendTextMessageAsync(context.ChatID, "非常抱歉，暂时还不支持多张组图，请发送一张图片。");
            return await Task.FromResult(false);
        }
    }
}
