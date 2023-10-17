using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Pipeline.Abstracts
{
    /// <summary>
    /// 流水线名称
    /// </summary>
    /// <remarks>
    /// 实现这个接口，调用 <see cref="IPipelineController{T}.NextPipelineName"/> 时，可以显示名称
    /// </remarks>
    public interface IPipelineName
    {
        /// <summary>
        /// 想要显示的名称
        /// </summary>
        public string Name { get; }
    }
}
