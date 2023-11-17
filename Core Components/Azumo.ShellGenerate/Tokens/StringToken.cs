using Azumo.ShellGenerate.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate.Tokens
{
    public class StringToken : TokenBase
    {
        private readonly string _value;
        public StringToken(string text)
        {
            _value = text;
        }

        public override string Generate()
        {
            return _value;
        }

    }
}
