using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.Core.BotBuilder;

/// <summary>
/// 
/// </summary>
public interface ITelegramModule
{
    /// <summary>
    /// 添加创建时服务
    /// </summary>
    /// <remarks>
    /// 创建时服务，用于执行 <see cref="Build(IServiceCollection, IServiceProvider)"/> 方法时使用的服务。
    /// </remarks>
    /// <param name="services">服务集合</param>
    public void AddBuildService(IServiceCollection services);

    /// <summary>
    /// 创建运行时服务
    /// </summary>
    /// <remarks>
    /// 可以使用 <paramref name="builderService"/> 参数来获取创建时服务。
    /// 可以使用创建时服务来创建，添加，处理运行时服务
    /// </remarks>
    /// <param name="services">运行时服务集合</param>
    /// <param name="builderService">创建时服务</param>
    public void Build(IServiceCollection services, IServiceProvider builderService);
}
