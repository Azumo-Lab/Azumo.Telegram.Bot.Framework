using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Azumo.Utils;

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
        var hasErr = false;
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
    public static bool WriteTo(this string str, Stream stream) => WriteTo(str, stream, Encoding.UTF8);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static bool WriteTo(this string str, Stream stream, Encoding encoding)
    {
        var hasErr = false;
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
        var hasErr = false;
        try
        {
            using (StreamWriter streamWriter = new(new FileStream(filePath, FileMode.OpenOrCreate)))
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
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string ReturnFilePathWriteTo(this string str, string filePath) => WriteTo(str, filePath) ? filePath : string.Empty;

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
        List<string> strings = [];
        if (Directory.Exists(str))
            strings = Directory.GetFiles(str, searchPattern, searchOption)?.ToList() ?? [];
        return strings;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string AppendTo(this string str, string value) => string.Concat(value, str);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string AppendTo(this string str, ref string value) => value = string.Concat(value, str);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Append(this string str, string value) => string.Concat(str, value);
}
