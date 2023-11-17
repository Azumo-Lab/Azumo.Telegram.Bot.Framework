using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate.Tokens
{
    public class ConcatToken : TokenBase
    {
        private readonly TokenBase AToken;
        private readonly TokenBase BToken;

        public ConcatToken(TokenBase a, TokenBase b) 
        { 
            AToken = a;
            BToken = b;
        }

        public override string Generate()
        {
            return $"{AToken.Generate()} | {BToken.Generate()}";
        }
    }
}
