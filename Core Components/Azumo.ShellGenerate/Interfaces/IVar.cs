using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate.Interfaces
{
    public interface IVar<T>
    {
        public T SetName(TokenBase name);

        public T SetValue(TokenBase value);
    }
}
