using Azumo.ShellGenerate.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate
{
    public abstract class TokenBase
    {
        public static TokenBase operator |(TokenBase a, TokenBase b)
        {
            return new ConcatToken(a, b);
        }

        public static TokenBase operator &(TokenBase a, TokenBase b)
        {
            return null!;
        }

        public static implicit operator TokenBase(string str)
        {
            return new StringToken(str);
        }

        public abstract string Generate();
    }
}
