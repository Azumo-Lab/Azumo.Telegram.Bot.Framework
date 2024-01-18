namespace Telegram.Bot.Framework.Users;

public interface ISecretGetter
{
    public Task<string> GetSecret();
}
