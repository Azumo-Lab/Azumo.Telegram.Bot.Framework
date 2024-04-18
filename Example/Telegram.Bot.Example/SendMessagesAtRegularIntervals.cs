using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Execs;
using Telegram.Bot.Types;

namespace Telegram.Bot.Example;

[DependencyInjection(ServiceLifetime.Singleton, Key = "SendMessagesAtRegularIntervals", ServiceType = typeof(ITask))]
internal class SendMessagesAtRegularIntervals : TimerTask
{
    public SendMessagesAtRegularIntervals() =>
        Interval = TimeSpan.FromSeconds(10);

    protected override async Task IntervalExecuteAsync(object input, CancellationToken token)
    {
        var inputObj = input as SendMessagesAtRegularIntervalsParamClass ?? throw new ArgumentException(null, nameof(input));

        _ = await inputObj.BotClient.SendTextMessageAsync(inputObj.SendUser, "Hello, World!", cancellationToken: token);
    }
}

internal class SendMessagesAtRegularIntervalsParamClass
{
    public ChatId SendUser { get; set; }

    public ITelegramBotClient BotClient { get; set; }
}
