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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Telegram.Bot.Framework.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class AESEncrypt
    {
        private static byte[] _AesKey;
        private static byte[] _AesIV;

        private byte[] _aesKey;
        private byte[] _aesIV;

        public AESEncrypt(string Password)
        {
            AESHelper.SetPassword(Password, out _aesKey, out _aesIV);
        }

        public byte[] Encrypt(string Text)
        {
            return AESHelper.EncryptStringToBytes_Aes(Text, _aesKey, _aesIV);
        }

        public string Decrypt(byte[] array)
        {
            return AESHelper.DecryptStringFromBytes_Aes(array, _aesKey, _aesIV);
        }

        public static byte[] StaticEncrypt(string Text)
        {
            if (_AesKey == null || _AesIV == null)
                throw new ArgumentNullException($"{nameof(_AesKey)}, {nameof(_AesIV)}");
            return AESHelper.EncryptStringToBytes_Aes(Text, _AesKey, _AesIV);
        }

        public static string StaticDecrypt(byte[] array)
        {
            if (_AesKey == null || _AesIV == null)
                throw new ArgumentNullException($"{nameof(_AesKey)}, {nameof(_AesIV)}");
            return AESHelper.DecryptStringFromBytes_Aes(array, _AesKey, _AesIV);
        }

        public static void SetPassword(string Password)
        {
            AESHelper.SetPassword(Password, out _AesKey, out _AesIV);
        }
    }
}
