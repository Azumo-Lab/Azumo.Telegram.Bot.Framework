﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate
{
    public abstract class Shell
    {
        private readonly List<TokenBase> __Tokens = [];

        protected abstract List<TokenBase> GenerateToken();

        public void Invoke(string outPath)
        {
            __Tokens.AddRange(GenerateToken());
            StringBuilder shellStr = new();
            foreach (var token in __Tokens)
            {
                var tokenStr = token.Generate();
                _ = shellStr.AppendLine(tokenStr);
            }
            Console.WriteLine(shellStr.ToString());
        }

        public static T Token<T>() where T : TokenBase => (T)Activator.CreateInstance(typeof(T))!;

    }
}
