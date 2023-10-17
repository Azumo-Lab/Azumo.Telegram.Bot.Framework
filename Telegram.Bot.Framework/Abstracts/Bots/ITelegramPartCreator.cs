using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    /// <summary>
    /// 可扩展的内容设置
    /// </summary>
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
