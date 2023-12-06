﻿namespace Azumo.ShellGenerate.Tokens
{
    internal class NumberToken : TokenBase
    {
        private readonly object _value;

        private NumberToken(object value) => _value = value;

        public NumberToken(int value) : this((object)value) { }

        public NumberToken(double value) : this((object)value) { }

        public NumberToken(long value) : this((object)value) { }

        public override string Generate() => _value.ToString()!;

        public override TokenBase Param(TokenBase token) => this;
    }
}
