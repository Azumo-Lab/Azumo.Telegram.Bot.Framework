using Microsoft.EntityFrameworkCore;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;

namespace MyChannel.Controllers
{
    internal class BlockUserAuth(MyDBContext __Context) : IAuthenticate
    {
        private readonly MyDBContext __Context = __Context;

        public HashSet<Enum> RoleName { get; set; } = [];

        public async Task<bool> IsAuthenticated(TGChat tGChat, AuthenticateAttribute authenticateAttribute)
        {
            if (RoleName.Count == 0)
            {
                UserInfo? userInfo = await __Context.Users.Where(x => x.ChatID == tGChat.ChatId).FirstOrDefaultAsync();
                if (userInfo == null || userInfo.Blocked)
                    _ = RoleName.Add(AuthEnum.NONE);
            }
            foreach (Enum role in RoleName)
                if (authenticateAttribute.RoleName.Contains(role))
                    return true;
            return false;
        }
    }
}
