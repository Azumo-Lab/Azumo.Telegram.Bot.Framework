using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    /// <summary>
    /// 可扩展的内容设置
    /// </summary>
    /// <remarks>
    /// 通过实现这个接口，来进行 <see cref="ITelegramBotBuilder"/> 接口的处理设置
    /// </remarks>
    public interface ITelegramPartCreator
    {
        /// <summary>
        /// 添加创建时服务
        /// </summary>
        /// <param name="services"></param>
        public void AddBuildService(IServiceCollection services);

        /// <summary>
        /// 创建运行时服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builderService"></param>
        public void Build(IServiceCollection services, IServiceProvider builderService);
    }
}
