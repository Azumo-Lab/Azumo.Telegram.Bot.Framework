using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Storage;

namespace Telegram.Bot.Framework.Core.Controller.Controller;

/// <summary>
/// 
/// </summary>
/// <param name="serviceProvider"></param>
internal class CommandScopeService(IServiceProvider serviceProvider) : ICommandScopeService
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IServiceProvider UserScopeServiceProvider = serviceProvider;

    /// <summary>
    /// 
    /// </summary>
    private IServiceScope? _serviceScope;

    /// <summary>
    /// 
    /// </summary>
    public IServiceProvider? Service => _serviceScope?.ServiceProvider;

    public ISession Session { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public void Create()
    {
        _serviceScope = UserScopeServiceProvider.CreateScope();
        Session = Service!.GetRequiredService<ISession>();
    }
    /// <summary>
    /// 
    /// </summary>
    public void Delete()
    {
        Session.Dispose();
        _serviceScope?.Dispose();
        _serviceScope = null;
    }

    public void DeleteOldCreateNew()
    {
        Delete();
        Create();
    }
}
