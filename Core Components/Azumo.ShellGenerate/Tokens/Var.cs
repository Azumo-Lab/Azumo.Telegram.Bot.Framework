using Azumo.ShellGenerate.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate.Tokens
{
    public class Var : TokenBase
    {
        private TokenBase Value { get; set; } = string.Empty;

        public override string Generate()
        {
            return $"{Value.Generate()}";
        }

        public override TokenBase Param(TokenBase token)
        {
            Value = token;
            return this;
        }
    }
}
