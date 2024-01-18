//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Text;
using System.Text.Encodings.Web;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;

namespace Telegram.Bot.Framework.InternalInterface;

/// <summary>
/// 
/// </summary>
[DependencyInjection(ServiceLifetime.Transient, typeof(IMessageBuilder))]
internal class MessageBuilder : IMessageBuilder
{
    private readonly List<IMessageContent> _Content = [];
    public IMessageBuilder Add(IMessageContent messageContent)
    {
        _Content.Add(messageContent);
        return this;
    }

    public string Build()
    {
        StringBuilder builder = new();
        foreach (var content in _Content)
            _ = builder.AppendLine(content.Build());
        return builder.ToString();
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
internal class ConcatMessage(IMessageContent a, IMessageContent b) : BaseMessage
{
    public override string Build() => $"{a.Build()}{b.Build()}";
}

/// <summary>
/// 
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
internal class SpaceConcatMessage(IMessageContent a, IMessageContent b) : BaseMessage
{
    public override string Build() => $"{a.Build()} {b.Build()}";
}

/// <summary>
/// 
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
internal class ConcatMessageWithSpace(BaseMessage a, BaseMessage b) : BaseMessage
{
    public override string Build() => $"{a.Build()} {b.Build()}";
}

/// <summary>
/// 
/// </summary>
public abstract class BaseMessage : IMessageContent
{
    public abstract string Build();

    public static implicit operator BaseMessage(string str) => new StringMessage(str);

    public static IMessageContent operator |(BaseMessage baseMessage, BaseMessage other) => (IMessageContent)baseMessage | other;

    public static IMessageContent operator |(string baseMessage, BaseMessage other) => (BaseMessage)baseMessage | other;

    public static IMessageContent operator |(IMessageContent baseMessage, BaseMessage other) => new ConcatMessage(baseMessage, other);

    public static IMessageContent operator |(BaseMessage baseMessage, IMessageContent other) => new ConcatMessage(baseMessage, other);

    public static IMessageContent operator &(BaseMessage baseMessage, BaseMessage other) => (IMessageContent)baseMessage & other;

    public static IMessageContent operator &(string baseMessage, BaseMessage other) => (BaseMessage)baseMessage & other;

    public static IMessageContent operator &(IMessageContent baseMessage, BaseMessage other) => new SpaceConcatMessage(baseMessage, other);

    public static IMessageContent operator &(BaseMessage baseMessage, IMessageContent other) => new SpaceConcatMessage(baseMessage, other);
}

/// <summary>
/// 添加普通的文本
/// </summary>
/// <param name="str"></param>
public class StringMessage(string str) : BaseMessage
{
    private readonly string __Str = HtmlEncoder.Default.Encode(str);
    public override string Build() => $"{__Str}";
}

/// <summary>
/// 添加一个Hash Tag标签
/// </summary>
/// <param name="tag"></param>
public class HashTagMessage(string tag) : BaseMessage
{
    private readonly string __Tag = tag;
    public override string Build() => (__Tag.StartsWith('#') ?
            new URLMessage(null, __Tag) :
            new URLMessage(null, $"#{__Tag}")).Build();
}

/// <summary>
/// 对文字加粗
/// </summary>
/// <param name="message"></param>
public class BoldMessage(string message) : BaseMessage
{
    private readonly string __Message = HtmlEncoder.Default.Encode(message);

    public override string Build() => $"<b>{__Message}</b>";
}

/// <summary>
/// 添加斜体
/// </summary>
/// <param name="message"></param>
public class ItalicMessage(string message) : BaseMessage
{
    private readonly string __Message = HtmlEncoder.Default.Encode(message);

    public override string Build() => $"<i>{__Message}</i>";
}

/// <summary>
/// 添加下划线
/// </summary>
/// <param name="message"></param>
public class UnderlineMessage(string message) : BaseMessage
{
    private readonly string __Message = HtmlEncoder.Default.Encode(message);

    public override string Build() => $"<u>{__Message}</u>";
}

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public class StrikethroughMessage(string message) : BaseMessage
{
    private readonly string __Message = HtmlEncoder.Default.Encode(message);

    public override string Build() => $"<s>{__Message}</s>";
}

/// <summary>
/// 添加一个隐藏消息
/// </summary>
/// <param name="message"></param>
public class SpoilerMessage(string message) : BaseMessage
{
    private readonly string __Message = HtmlEncoder.Default.Encode(message);

    public override string Build() => $"<span class=\"tg-spoiler\">{__Message}</span>";
}

/// <summary>
/// 添加URL类型消息
/// </summary>
/// <param name="url"></param>
/// <param name="text"></param>
public class URLMessage(string url, string text) : BaseMessage
{
    private readonly string __Text = HtmlEncoder.Default.Encode(text);

    public override string Build()
    {
        StringBuilder stringBuilder = new();
        _ = string.IsNullOrEmpty(url) ? stringBuilder.Append($"<a") : stringBuilder.Append($"<a href=\"{url}\"");
        _ = stringBuilder.Append($">{__Text}</a>");
        return stringBuilder.ToString();
    }
}

/// <summary>
/// 添加代码类型消息
/// </summary>
/// <param name="code"></param>
public class CodeMessage(string code) : BaseMessage
{
    private readonly string __Code = HtmlEncoder.Default.Encode(code);

    public override string Build() => $"<code>{__Code}</code>";
}

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public class PreMessage(string message) : BaseMessage
{
    private readonly string __Message = HtmlEncoder.Default.Encode(message);

    public override string Build() => $"<pre>{__Message}</pre>";
}

/// <summary>
/// 添加新的一行
/// </summary>
public class NewLineMessage() : BaseMessage
{
    public override string Build() => string.Empty;
}
