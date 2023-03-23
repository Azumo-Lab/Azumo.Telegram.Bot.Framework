using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Helper
{
    public static class BoolHelper
    {
        public static bool True(this ref bool value)
        {
            value = true; 
            return value;
        }

        public static bool False(this ref bool value)
        {
            value = false;
            return value;
        }
    }
}
