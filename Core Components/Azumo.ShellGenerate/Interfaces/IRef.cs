﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.ShellGenerate.Interfaces
{
    public interface IRef
    {
        public TokenBase CreateRef(string name);

        public string Generate();
    }
}