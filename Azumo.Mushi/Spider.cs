using Azumo.Pipeline;
using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Azumo.Mushi
{
    public abstract class Spider
    {
        public async Task StartAsync()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddScoped<DataContext, NormalDataContext>()
                .BuildServiceProvider();

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var __Pipeline =
                AddProcessFlow(PipelineFactory.CreateIPipelineBuilder<DataContext>())
                .BuilderPipelineController();

                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                context.StartURL.AddRange(StartPages());

                var dataContext = 
                    await __Pipeline.SwitchTo(string.Empty, context);
               
                await Task.CompletedTask;
            }
        }

        protected abstract IPipelineBuilder<DataContext> AddProcessFlow(IPipelineBuilder<DataContext> builder);

        protected abstract List<string> StartPages();
    }
}
