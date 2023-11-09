using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    public interface ITelegramService
    {
        public void AddServices(IServiceCollection services);
    }
}
