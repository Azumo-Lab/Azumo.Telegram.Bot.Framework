﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    internal enum ResultEnum
    {
        NoStatus,
        SendMessage,
        ReceiveParameters,
        NextParam,
        Finish,
    }
}
