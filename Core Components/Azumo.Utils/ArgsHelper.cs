using System.Collections.Generic;

namespace Azumo.Utils
{
    public static class ArgsHelper
    {
        public static Dictionary<string, string> ToDictionary(string[] args)
        {
            Dictionary<string, string> dic = [];
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (!arg.StartsWith('-'))
                    continue;

                dic.Add(arg.ToLower(), args[++i].ToLower());
            }
            return dic;
        }
    }
}
