using Azumo.ShellGenerate.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate.Tokens
{
    public class Var : TokenBase, IVar<Var>
    {
        public TokenBase Name { get; set; } = string.Empty;

        public TokenBase Value { get; set; } = string.Empty;

        public override string Generate()
        {
            if (string.IsNullOrEmpty(Value?.Generate()))
                return $"${Name?.Generate()}";
            else
                return $"${Name?.Generate()}={Value?.Generate()}";
        }

        public Var SetName(TokenBase name)
        {
            Name = name;
            return this;
        }

        public Var SetValue(TokenBase value)
        {
            Value = value;
            return this;
        }
    }
}
