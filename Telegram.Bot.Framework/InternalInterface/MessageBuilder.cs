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

namespace Telegram.Bot.Framework.InternalInterface
{
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
            foreach (IMessageContent content in _Content)
                _ = builder.AppendLine(content.Build());
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

    internal class ConcatMessageWithSpace(BaseMessage a, BaseMessage b) : BaseMessage
    {
        public override string Build()
        {
            return $"{a.Build()} {b.Build()}";
        }
    }

    public abstract class BaseMessage : IMessageContent
    {
        public abstract string Build();

        public static implicit operator BaseMessage(string str)
        {
            return new StringMessage(str);
        }

        public static BaseMessage operator |(BaseMessage baseMessage, BaseMessage other)
        {
            return new ConcatMessage(baseMessage, other);
        }

        public static BaseMessage operator &(BaseMessage baseMessage, BaseMessage other)
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
        private readonly string __Tag = HtmlEncoder.Default.Encode(tag);
        public string Build()
        {
            return __Tag.StartsWith('#') ? $"<a>{__Tag}</a>" : $"<a>#{__Tag}</a>";
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
                _ = stringBuilder.Append($"<a");
            else
                _ = stringBuilder.Append($"<a href=\"{url}\"");
            _ = stringBuilder.Append($">{__Text}</a>");
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
