﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Core.Execs;
public abstract class BackgroundTasks : ITask
{
    public Task Exec() => throw new NotImplementedException();
}
