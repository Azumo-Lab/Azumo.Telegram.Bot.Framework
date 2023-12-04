using Azumo.ShellGenerate.Interfaces;
using Azumo.ShellGenerate.Tokens;

namespace Azumo.ShellGenerate
{
    public abstract class TokenBase : IRef, IParam
    {
        public static TokenBase operator |(TokenBase a, TokenBase b)
        {
            return new ConcatToken(a, b);
        }

        public static TokenBase operator &(TokenBase a, TokenBase b)
        {
            return a | b;
        }

        public static implicit operator TokenBase(string str)
        {
            return new StringToken(str);
        }

        public static implicit operator TokenBase(int i)
        {
            return new NumberToken(i);
        }

        public TokenBase CreateRef(string name)
        {
            return new RefToken(this, name);
        }

        public abstract string Generate();
        public abstract TokenBase Param(TokenBase token);
    }
}
