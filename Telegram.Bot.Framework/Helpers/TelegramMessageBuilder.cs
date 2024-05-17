//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Helpers
{
    /// <summary>
    /// Telegram 消息创建器
    /// </summary>
    public abstract class TelegramMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public ParseMode ParseMode { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        private readonly StringBuilder stringBuilder = new StringBuilder();

        /// <summary>
        /// 
        /// </summary>
        public static TelegramMessageBuilder Markdown => new MarkdownV2MessageBuilder();

        /// <summary>
        /// 
        /// </summary>
        public static TelegramMessageBuilder Html => new HtmlMessageBuilder();

        /// <summary>
        /// 
        /// </summary>
        private class HtmlMessageBuilder : TelegramMessageBuilder
        {
            public HtmlMessageBuilder() : base() => 
                ParseMode = ParseMode.Html;

            public override TelegramMessageBuilder Blockquote(string text) => Append($"<blockquote>{text}</blockquote>");
            public override TelegramMessageBuilder Bold(string text) => Append($"<b>{text}</b>");
            public override TelegramMessageBuilder Code(string text) => Append($"<code>{text}</code>");
            public override TelegramMessageBuilder HashTag(string text) => Append($"<a>{text}</a>");
            public override TelegramMessageBuilder Italic(string text) => Append($"<i>{text}</i>");
            public override TelegramMessageBuilder Link(string text, string url) => Append($"<a href=\"{url}\">{text}</a>");
            public override TelegramMessageBuilder LinkUser(string text, long userid) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Pre(string text) => Append($"<pre>{text}</pre>");
            public override TelegramMessageBuilder PreCode(string text, Language language) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Spoiler(string text) => Append($"<tg-spoiler>{text}</tg-spoiler>");
            public override TelegramMessageBuilder Strikethrough(string text) => Append($"<s>{text}</s>");
            public override TelegramMessageBuilder Underline(string text) => Append($"<u>{text}</u>");
        }

        private class MarkdownV2MessageBuilder : TelegramMessageBuilder
        {
            public MarkdownV2MessageBuilder() : base() =>
                ParseMode = ParseMode.MarkdownV2;

            public override TelegramMessageBuilder Blockquote(string text) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Bold(string text) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Code(string text) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder HashTag(string text) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Italic(string text) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Link(string text, string url) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder LinkUser(string text, long userid) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Pre(string text) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder PreCode(string text, Language language) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Spoiler(string text) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Strikethrough(string text) => throw new System.NotImplementedException();
            public override TelegramMessageBuilder Underline(string text) => throw new System.NotImplementedException();
        }

        private class MarkdownMessageBuilder : TelegramMessageBuilder
        {
            public MarkdownMessageBuilder() : base() =>
                ParseMode = ParseMode.Markdown;

            public override TelegramMessageBuilder Blockquote(string text) => Append(string.Empty);
            public override TelegramMessageBuilder Bold(string text) => Append($"*{text}*");
            public override TelegramMessageBuilder Code(string text) => Append($"`{text}`");
            public override TelegramMessageBuilder HashTag(string text) => Append(string.Empty);
            public override TelegramMessageBuilder Italic(string text) => Append($"_{text}_");
            public override TelegramMessageBuilder Link(string text, string url) => Append($"[{text}]({url})");
            public override TelegramMessageBuilder LinkUser(string text, long userid) => Append($"[{text}](tg://user?id={userid})");
            public override TelegramMessageBuilder Pre(string text) => Append($"```").NewLine().Append(text).NewLine().Append("```");
            public override TelegramMessageBuilder PreCode(string text, Language language) => Append($"```{language.ToString().ToLower()}").NewLine().Append(text).NewLine().Append("```");
            public override TelegramMessageBuilder Spoiler(string text) => Append(string.Empty);
            public override TelegramMessageBuilder Strikethrough(string text) => Append(string.Empty);
            public override TelegramMessageBuilder Underline(string text) => Append(string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => 
            stringBuilder.ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual TelegramMessageBuilder Append(string text)
        {
            stringBuilder.Append(text);
            return this;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual TelegramMessageBuilder NewLine()
        {
            stringBuilder.AppendLine();
            return this;
        }

        /// <summary>
        /// 创建一个带有连接的文本
        /// </summary>
        /// <param name="text">显示的文本</param>
        /// <param name="url">链接</param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Link(string text, string url);

        /// <summary>
        /// 创建一个带有连接的文本
        /// </summary>
        /// <param name="text">显示的文本</param>
        /// <param name="userid">链接</param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder LinkUser(string text, long userid);

        /// <summary>
        /// 加粗文字
        /// </summary>
        /// <param name="text">想要加粗的文字</param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Bold(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Italic(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Underline(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Strikethrough(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Spoiler(string text);

        /// <summary>
        /// 代码文字块
        /// </summary>
        /// <param name="text">代码文字</param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Code(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Pre(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract TelegramMessageBuilder PreCode(string text, Language language);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder Blockquote(string text);

        /// <summary>
        /// Tag标签
        /// </summary>
        /// <param name="text">标签内容</param>
        /// <returns>处理后的文本</returns>
        public abstract TelegramMessageBuilder HashTag(string text);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// 
        /// </summary>
        CSharp,

        /// <summary>
        /// 
        /// </summary>
        Python,
    }
}
