using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Storage;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal interface ICommandScopeService
{
    public IServiceProvider? Service { get; }

    public ISession Session { get; }

    public void Create();

    public void Delete();

    public void DeleteOldCreateNew();
}
