using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller.Results
{
    internal class PhotoMessageResult : MessageResult
    {
        private readonly string[] PhotoPaths;

        private readonly string? Caption;

        private readonly ButtonResult[]? ButtonResults;
        public PhotoMessageResult(string photoPath, string? caption = null, ButtonResult[]? buttonResults = null)
        {
            PhotoPaths = new string[] { photoPath };
            Caption = caption;
            ButtonResults = buttonResults;
        }

        public PhotoMessageResult(string[] photoPaths, string? caption = null, ButtonResult[]? buttonResults = null)
        {
            PhotoPaths = photoPaths;
            Caption = caption;
            ButtonResults = buttonResults;
        }

        public override async Task ExecuteResultAsync(TelegramActionContext context)
        {
            var buttons = new List<InlineKeyboardButton>();
            if (ButtonResults != null && ButtonResults.Length > 0)
            {
                var callbackManager = context.ServiceProvider.GetRequiredService<ICallBackManager>();
                foreach (var item in ButtonResults)
                    buttons.Add(callbackManager.CreateCallBackButton(item));
            }
            if (PhotoPaths.Length == 1)
            {
                using (var bufferedStream = new BufferedStream(new FileStream(PhotoPaths[0], FileMode.Open), 4096))
                {
                    if (buttons.Count == 0)
                        await context.TelegramBotClient.SendPhotoAsync(context.ChatId, InputFile.FromStream(bufferedStream), caption: Caption, parseMode: Types.Enums.ParseMode.Html);
                    else
                        await context.TelegramBotClient.SendPhotoAsync(context.ChatId, InputFile.FromStream(bufferedStream), caption: Caption, replyMarkup:
                                new InlineKeyboardMarkup(buttons));
                }
            }
            else
            {
                var streams = PhotoPaths.Select(x => new BufferedStream(new FileStream(x, FileMode.Open), 4096))
                    .ToList();
                await context.TelegramBotClient.SendMediaGroupAsync(context.ChatId, streams.Select(x => new InputMediaPhoto(InputFile.FromStream(x)) { Caption = Caption, ParseMode = Types.Enums.ParseMode.Html }).ToArray());
            }
        }
    }
}
