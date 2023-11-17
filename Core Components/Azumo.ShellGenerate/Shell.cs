using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Azumo.ShellGenerate
{
    public abstract class Shell
    {
        private readonly List<TokenBase> __Tokens = new List<TokenBase>();

        protected abstract List<TokenBase> GenerateToken();

        public void Invoke(string outPath)
        {
            __Tokens.AddRange(GenerateToken());
            StringBuilder shellStr = new StringBuilder();
            foreach (TokenBase token in __Tokens)
            {
                string tokenStr = token.Generate();
                shellStr.Append(tokenStr);
            }
            File.WriteAllText(outPath, shellStr.ToString());
        }

        public T Token<T>() where T : TokenBase
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
