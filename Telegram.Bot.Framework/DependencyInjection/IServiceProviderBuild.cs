using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.DependencyInjection
{
    public interface IServiceProviderBuild
    {
        public IServiceProvider Build();
    }
}
