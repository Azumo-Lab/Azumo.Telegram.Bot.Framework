//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using System.IO;

namespace Telegram.Bot.Framework.Controller.Fragments
{
    internal class ImageMessage : IMessageFragment
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public ImageMessage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                throw new FileNotFoundException(imagePath);

            Data = new Stream[]
            {
                new BufferedStream(new FileStream(imagePath, FileMode.Open, FileAccess.Read), 4096),
            };
            DataInfo = Path.GetFileName(imagePath);
        }
        public FragmentType FragmentType => FragmentType.IsPhoto;

        public string DataInfo { get; set; }

        public Stream[]? Data { get; set; }
    }
}
