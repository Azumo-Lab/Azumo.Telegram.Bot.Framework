namespace Telegram.Bot.Framework.Helpers;

/// <summary>
/// 
/// </summary>
public static class MessageMaker
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string Link(string text, string url) =>
        $"<a href=\"{url}\">{text}</a>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Bold(string text) =>
        $"<b>{text}</b>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Italic(string text) =>
        $"<i>{text}</i>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Underline(string text) =>
        $"<u>{text}</u>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Strikethrough(string text) =>
        $"<s>{text}</s>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Spoiler(string text) =>
        $"<tg-spoiler>{text}</tg-spoiler>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Code(string text) =>
        $"<code>{text}</code>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Pre(string text) =>
        $"<pre>{text}</pre>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Blockquote(string text) =>
        $"<blockquote>{text}</blockquote>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string HashTag(string text) =>
        $"<a>{text}</a>";
}
