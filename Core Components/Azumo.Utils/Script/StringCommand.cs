using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Azumo.Utils.Script
{
    public class StringCommand
    {
        public static implicit operator StringCommand(string s)
        {
            return new StringCommand();
        }

        public static StringCommand operator |(string a, StringCommand b)
        {
            return new StringCommand();
        }

        public static StringCommand operator &(StringCommand a, StringCommand b)
        {
            return null!;
        }
    }
}
