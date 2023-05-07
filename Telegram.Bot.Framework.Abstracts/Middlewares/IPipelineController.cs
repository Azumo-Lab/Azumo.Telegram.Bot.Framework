using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Session;

namespace Telegram.Bot.Framework.Abstracts.Middlewares
{
    /// <summary>
    /// 流水线控制器
    /// </summary>
    public interface IPipelineController
    {
        #region 框架内部方法
        internal void SetNextHandle(MiddlewareDelegate handle);
        #endregion

        /// <summary>
        /// 判断是否已经有了内容
        /// </summary>
        public bool HasAnyPipeline { get; }

        /// <summary>
        /// 执行下一条作业
        /// </summary>
        /// <returns></returns>
        public Task Next(ITelegramSession Session);

        /// <summary>
        /// 切换一条流水线
        /// </summary>
        /// <param name="pipelineName">名称</param>
        public void ChangePipeline(string pipelineName = null!);

        /// <summary>
        /// 创建一条新的流水线
        /// </summary>
        /// <param name="pipelineName">名称</param>
        /// <param name="piplineBuilder"></param>
        public void AddPipeline(string pipelineName, IPipelineBuilder piplineBuilder);

        /// <summary>
        /// 创建一条新的流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="piplineBuilder"></param>
        /// <returns></returns>
        public bool TryAddPipeline(string pipelineName, IPipelineBuilder piplineBuilder);
    }
}
