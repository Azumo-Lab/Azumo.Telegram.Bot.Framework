using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.DependencyInjection
{
    public interface IServiceCollection : ICollection<TelegramServiceDescriptor>, IEnumerable<TelegramServiceDescriptor>, IEnumerable, IList<TelegramServiceDescriptor>
    {
    }
}
