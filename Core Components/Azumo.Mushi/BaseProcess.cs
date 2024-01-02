using Azumo.Pipeline.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Mushi
{
    public abstract class BaseProcess : IProcessAsync<DataContext>
    {
        protected static HttpClient HttpClient { get; } = new HttpClient();

        public virtual async Task<DataContext> ExecuteAsync(DataContext t, IPipelineController<DataContext> pipelineController) => 
            !await Process(t) ? await pipelineController.StopAsync(t) : await pipelineController.NextAsync(t);

        protected abstract Task<bool> Process(DataContext dataContext);

        protected static async Task<string> GetHtml(string url)
        {
            var httpResponseMessage = await HttpClient.GetAsync(url);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
