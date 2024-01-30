using Azumo.PipelineMiddleware;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controllers.Abstracts;
using Telegram.Bot.Framework.Controllers.Abstracts.Internals;

namespace Telegram.Bot.Framework.Controllers.Internals;
internal class BaseGetParamters(List<IParameterCapturer> parameterCapturers) : IGetParameters
{
    private readonly List<object?> _parameters = [];

    private readonly List<IParameterCapturer> _parameterCapturers = parameterCapturers ?? [];

    private readonly List<IParameterCapturer> _copys = [];

    public async Task<EnumParametersResults> GetParametersAsync(TelegramUserChatContext telegramUserChatContext)
    {
        var parameterCapturer = _copys.FirstOrDefault();
        if (parameterCapturer == null)
            return EnumParametersResults.Success;

        EnumParametersResults results;

        switch (parameterCapturer.Results)
        {
            case EnumParametersResults.WaitingForInput:
            case EnumParametersResults.NONE:
                results = await ParamPipeline.pipelineController.Execute(parameterCapturer.Results,
                new ParamPipeline.Models
                {
                    ParameterCapturer = parameterCapturer,
                    TelegramUserChatContext = telegramUserChatContext,
                    Parameters = _parameters,
                });
                break;
            default:
                results = parameterCapturer.Results;
                _copys.RemoveAt(0);
                break;
        }

        return results;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public object?[] GetParams() =>
        _parameters.ToArray();

    public void Init()
    {
        _copys.Clear();
        _parameterCapturers.ForEach(x => x.Init());
        _copys.AddRange(_parameterCapturers);
    }
}

internal static class ParamPipeline
{
    public static IPipelineController<Models, Task<EnumParametersResults>>
        pipelineController = PipelineFactory
        .GetPipelineBuilder<Models, Task<EnumParametersResults>>()
        .NewPipeline(EnumParametersResults.WaitingForInput)
        .Use(new GetParam())
        .NewPipeline(EnumParametersResults.NONE)
        .Use(new SendMessage())
        .Build();

    public class Models
    {
        public TelegramUserChatContext TelegramUserChatContext { get; set; } = null!;

        public IParameterCapturer ParameterCapturer { get; set; } = null!;

        public List<object?> Parameters { get; set; } = null!;
    }

    private class GetParam : IMiddleware<Models, Task<EnumParametersResults>>
    {
        public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

        public Task<EnumParametersResults> Execute(Models input, IPipelineController<Models, Task<EnumParametersResults>> pipelineController)
        {
            input.Parameters.Add(input.ParameterCapturer.GetParam(input.TelegramUserChatContext));
            return Task.FromResult(input.ParameterCapturer.Results);
        }
    }

    private class SendMessage : IMiddleware<Models, Task<EnumParametersResults>>
    {
        public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

        public async Task<EnumParametersResults> Execute(Models input, IPipelineController<Models, Task<EnumParametersResults>> pipelineController)
        {
            await input.ParameterCapturer.SendMessageAsync(input.TelegramUserChatContext);
            return input.ParameterCapturer.Results;
        }
    }
}
