using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Session;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Middlewares
{
    public abstract class AbsUpdateTypeMiddlewarePipeline : IMiddlewarePipeline
    {
        private readonly IServiceProvider __ServiceProvider;
        private readonly List<(string pipelineName, IPipelineBuilder pipelineBuilder)> __PipelineBuilderList = 
            new List<(string pipelineName, IPipelineBuilder pipelineBuilder)>();

        /// <summary>
        /// 执行的类型
        /// </summary>
        public abstract UpdateType InvokeType { get; }
        private string __InvokeTypeString = string.Empty;
        protected string InvokeTypeStr => __InvokeTypeString.GetValue(ref __InvokeTypeString, InvokeType.ToString());

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        public AbsUpdateTypeMiddlewarePipeline(IServiceProvider ServiceProvider)
        {
            __ServiceProvider = ServiceProvider;
            AddMiddlewareHandleTemplate(__ServiceProvider);
        }

        /// <summary>
        /// 添加一个模板方法
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        private void AddMiddlewareHandleTemplate(IServiceProvider ServiceProvider)
        {
            List<IMiddlewareTemplate> middlewareTemplates = ServiceProvider.GetServices<IMiddlewareTemplate>()?.ToList() ?? new List<IMiddlewareTemplate>();

            middlewareTemplates.ForEach(x => x.BeforeAddMiddlewareHandles(ServiceProvider));

            AddMiddlewareHandles(ServiceProvider);

            middlewareTemplates.ForEach(x => x.AfterAddMiddlewareHandles(ServiceProvider));
        }

        /// <summary>
        /// 添加中间件，创建中间件流水线
        /// </summary>
        /// <param name="ServiceProvider">DI服务</param>
        protected virtual void AddMiddlewareHandles(IServiceProvider ServiceProvider) { }

        /// <summary>
        /// 用于向流水线中添加中间件
        /// </summary>
        protected virtual void AddPipelineBuilder(string name, IPipelineBuilder pipelineBuilder)
        {
            __PipelineBuilderList.Add((name, pipelineBuilder));
        }

        /// <summary>
        /// 执行这个流水线
        /// </summary>
        /// <param name="context">访问的请求对话</param>
        /// <returns>异步可等待的Task</returns>
        public async Task Execute(ITelegramSession Session)
        {
            await InvokeAction(Session);
            // 新创建一个IPipelineController对象
            IPipelineController __PipelineController = Session.UserService.GetRequiredService<IPipelineController>();
            if (!__PipelineController.HasAnyPipeline)
            {
                // 尝试添加
                foreach ((string pipelineName, IPipelineBuilder pipelineBuilder) in __PipelineBuilderList)
                    __PipelineController.TryAddPipeline(pipelineName, pipelineBuilder);
            }
            // 切换到主分支
            __PipelineController.ChangePipeline(InvokeTypeStr);
            // 开始处理
            await __PipelineController.Next(Session);
        }

        /// <summary>
        /// 每次执行前的前置执行操作
        /// </summary>
        /// <param name="context">访问的请求对话</param>
        /// <returns>无</returns>
        protected virtual async Task InvokeAction(ITelegramSession Session)
        {
            await Task.CompletedTask;
        }
    }
}
