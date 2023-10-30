using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.ChannelManager
{
    internal static class Secret
    {
        public static IConfiguration Configuration
#if DEBUG
            = new ConfigurationBuilder().AddUserSecrets("f604f3a1-69dc-44ef-94df-78a90a51497f").Build();
#else
            = new ConfigurationBuilder().AddJsonFile("appconfig.json").Build();
#endif

        public static string AdminPassword = Configuration.GetSection(nameof(AdminPassword)).Value ?? string.Empty;

        public static string Token = Configuration.GetSection(nameof(Token)).Value ?? string.Empty;
    }
}
