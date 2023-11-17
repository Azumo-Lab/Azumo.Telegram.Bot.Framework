using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate.Interfaces
{
    public interface IParam<T>
    {
        public T Param(TokenBase token);
    }
}
