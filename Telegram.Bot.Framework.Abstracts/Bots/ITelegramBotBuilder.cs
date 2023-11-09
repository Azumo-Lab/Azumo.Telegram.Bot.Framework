namespace Telegram.Bot.Framework.Abstracts.Bots
{
    /// <summary>
    /// 用于构建 <see cref="ITelegramBot"/> 接口
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface ITelegramBotBuilder
    {
        /// <summary>
        /// 添加内容设置
        /// </summary>
        /// <remarks>
        /// 通过添加实现了 <see cref="ITelegramPartCreator"/> 接口的类，来处理和添加相对应的服务
        /// </remarks>
        /// <param name="telegramPartCreator">可扩展的内容设置 <see cref="ITelegramPartCreator"/> </param>
        /// <returns></returns>
        public ITelegramBotBuilder AddTelegramPartCreator(ITelegramPartCreator telegramPartCreator);

        /// <summary>
        /// 设置完成后，进行 <see cref="ITelegramBot"/> 对象的构建
        /// </summary>
        /// <remarks>
        /// 完成所有的服务构建后，开始构建 <see cref="ITelegramBot"/> 对象
        /// </remarks>
        /// <returns> 构建 <see cref="ITelegramBot"/> 对象 </returns>
        public ITelegramBot Build();
    }
}
