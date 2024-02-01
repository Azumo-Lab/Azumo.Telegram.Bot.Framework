using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Core.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
public class BotCommandAttribute : Attribute
{
    public string BotCommand { get; }

    public string Description { get; set; } = "No details";

    public BotCommandAttribute(string botCommand)
    {
        ArgumentException.ThrowIfNullOrEmpty(botCommand, nameof(botCommand));

        if (!botCommand.StartsWith('/'))
            botCommand = $"/{botCommand}";
        BotCommand = botCommand;
    }
}
