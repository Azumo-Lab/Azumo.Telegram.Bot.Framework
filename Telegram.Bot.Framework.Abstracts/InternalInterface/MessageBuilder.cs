using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.InternalInterface
{
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IMessageBuilder))]
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
            foreach (IMessageContent content in _Content)
                builder.AppendLine(content.Build());
            return builder.ToString();
        }
    }

    internal class ConcatMessage(BaseMessage a, BaseMessage b) : BaseMessage
    {
        public override string Build()
        {
            return $"{a.Build()}{b.Build()}";
        }
    }

    public abstract class BaseMessage : IMessageContent
    {
        public abstract string Build();

        public static implicit operator BaseMessage(string str)
        {
            return new StringMessage(str);
        }

        public static  BaseMessage operator | (BaseMessage baseMessage, BaseMessage other)
        {
            return new ConcatMessage(baseMessage, other);
        }
    }

    public class StringMessage(string str) : BaseMessage
    {
        private readonly string __Str = HtmlEncoder.Default.Encode(str);
        public override string Build()
        {
            return $"{__Str}";
        }
    }

    public class HashTagMessage(string tag) : IMessageContent
    {
        private string __Tag = HtmlEncoder.Default.Encode(tag);
        public string Build()
        {
            if (__Tag.StartsWith('#'))
                return $"<a>{__Tag}</a>";
            else
                return $"<a>#{__Tag}</a>";
        }
    }

    public class BoldMessage(string message) : IMessageContent
    {
        private readonly string __Message = HtmlEncoder.Default.Encode(message);

        public string Build()
        {
            return $"<b>{__Message}</b>";
        }
    }

    public class ItalicMessage(string message) : IMessageContent
    {
        private readonly string __Message = HtmlEncoder.Default.Encode(message);

        public string Build()
        {
            return $"<i>{__Message}</i>";
        }
    }

    public class UnderlineMessage(string message) : IMessageContent
    {
        private readonly string __Message = HtmlEncoder.Default.Encode(message);

        public string Build()
        {
            return $"<u>{__Message}</u>";
        }
    }

    public class StrikethroughMessage(string message) : IMessageContent
    {
        private readonly string __Message = HtmlEncoder.Default.Encode(message);

        public string Build()
        {
            return $"<s>{__Message}</s>";
        }
    }

    public class SpoilerMessage(string message) : IMessageContent
    {
        private readonly string __Message = HtmlEncoder.Default.Encode(message);

        public string Build()
        {
            return $"<span class=\"tg-spoiler\">{__Message}</span>";
        }
    }

    public class URLMessage(string url, string text) : BaseMessage
    {
        private readonly string __Text = HtmlEncoder.Default.Encode(text);

        public override string Build()
        {
            StringBuilder stringBuilder = new();
            if (string.IsNullOrEmpty(url))
                stringBuilder.Append($"<a");
            else
                stringBuilder.Append($"<a href=\"{url}\"");
            stringBuilder.Append($">{__Text}</a>");
            return stringBuilder.ToString();
        }
    }

    public class CodeMessage(string code) : IMessageContent
    {
        private readonly string __Code = HtmlEncoder.Default.Encode(code);

        public string Build()
        {
            return $"<code>{__Code}</code>";
        }
    }

    public class PreMessage(string message) : IMessageContent
    {
        private readonly string __Message = HtmlEncoder.Default.Encode(message);


        public string Build()
        {
            return $"<pre>{__Message}</pre>";
        }
    }

    public class NewLineMessage() : IMessageContent
    {
        public string Build()
        {
            return string.Empty;
        }
    }
}
