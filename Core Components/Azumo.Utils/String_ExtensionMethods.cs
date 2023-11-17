﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Azumo.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class String_ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public static bool WriteTo(this string str, TextWriter writer)
        {
            bool hasErr = false;
            try
            {
                writer.Write(str);
            }
            catch (Exception)
            {
                hasErr = true;
            }
            return !hasErr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool WriteTo(this string str, Stream stream)
        {
            return WriteTo(str, stream, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool WriteTo(this string str, Stream stream, Encoding encoding)
        {
            bool hasErr = false;
            try
            {
                stream.Write(encoding.GetBytes(str));
            }
            catch (Exception)
            {
                hasErr = true;
            }
            return !hasErr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool WriteTo(this string str, string filePath)
        {
            bool hasErr = false;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate)))
                {
                    streamWriter.Write(str);
                }
            }
            catch (Exception)
            {
                hasErr = true;
            }
            return !hasErr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static bool ReadToFileInfo(this string str, out FileInfo? fileInfo)
        {
            fileInfo = new FileInfo(str);
            return fileInfo != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static bool ReadToFileStream(this string str, out FileStream? fileStream)
        {
            fileStream = new FileStream(str, FileMode.OpenOrCreate);
            return fileStream != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> ListFiles(this string str, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            List<string> strings = new List<string>();
            if (Directory.Exists(str))
                strings = Directory.GetFiles(str, searchPattern, searchOption)?.ToList() ?? new List<string>();
            return strings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AppendTo(this string str, string value)
        {
            return string.Concat(value, str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AppendTo(this string str, ref string value)
        {
            return value = string.Concat(value, str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Append(this string str, string value)
        {
            return string.Concat(str, value);
        }
    }
}
