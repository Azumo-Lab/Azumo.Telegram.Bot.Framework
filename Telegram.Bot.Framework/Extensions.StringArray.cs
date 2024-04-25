using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework;
public static partial class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ToDictionary(this string[] args)
    {
        Dictionary<string, string> result = [];
        for (var i = 0; i < args.Length; i++)
            if (i + 1 >= args.Length)
                result.Add(args[i], string.Empty);
            else
                result.Add(args[i], args[++i]);
        return result;
    }
}
