﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Azumo.Lang
{
    internal abstract class ASTree : IEnumerable<ASTree>
    {
        public IEnumerator<ASTree> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
