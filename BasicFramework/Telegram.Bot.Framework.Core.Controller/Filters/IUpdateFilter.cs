using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.Filters;
public interface IUpdateFilter
{
    public bool Filter(Update update);
}
