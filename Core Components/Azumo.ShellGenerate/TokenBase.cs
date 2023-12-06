using Azumo.ShellGenerate.Interfaces;
using Azumo.ShellGenerate.Tokens;

namespace Azumo.ShellGenerate
{
    public abstract class TokenBase : IRef, IParam
    {
        public static TokenBase operator |(TokenBase a, TokenBase b) => new ConcatToken(a, b);

        public static TokenBase operator &(TokenBase a, TokenBase b) => a | b;

        public static implicit operator TokenBase(string str) => new StringToken(str);

        public static implicit operator TokenBase(int i) => new NumberToken(i);

        public TokenBase CreateRef(string name) => new RefToken(this, name);

        public abstract string Generate();
        public abstract TokenBase Param(TokenBase token);
    }
}
