using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Telegram.Bot.Framework.Abstract.Channels;
using Telegram.Bot.Framework.Authentication.Attribute;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Attribute;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Channel.Controllers
{
    /// <summary>
    /// 列出频道，需要管理员权限
    /// </summary>
    [Authentication(BotCommandScopeType.Chat, ChatUser = ChatUser.Admin)]
    public class ChannelListController : TelegramController
    {
        /// <summary>
        /// 获取展示用户在Bot注册的所有频道的列表。
        /// (注册:把Bot拉进频道管理员)
        /// </summary>
        /// <returns></returns>
        [BotCommand("ListChannel", Description = "展示所有注册的频道")]
        public async Task ListChannel()
        {
            // 获取注册的所有频道
            IChannelManager channelManager = Session.UserService.GetService<IChannelManager>()!;
            ChatId[] chats = channelManager.GetActiveChannel(Session.User);

            // 从频道的ChatID获取频道的信息
            List<(ChatId chatID, string chatName)> chatInfo = new List<(ChatId chatID, string chatName)>();
            foreach (ChatId chatId in chats)
            {
                Chat chat = await Session.BotClient.GetChatAsync(chatId);
                chatInfo.Add((chat.Id, chat.Title ?? string.Empty));
            }

            // 组装频道信息
            StringBuilder stringBuilder = new StringBuilder();
            foreach ((ChatId chatID, string chatName) in chatInfo)
            {
                stringBuilder.AppendLine($"{chatID} : {chatName}");
            }

            // 发送频道信息
            await Session.BotClient.SendTextMessageAsync(Session.User.ChatID!, stringBuilder.ToString());
        }
    }
}
