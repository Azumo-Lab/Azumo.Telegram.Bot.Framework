using Azumo.PipelineMiddleware;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controller.Params.ParamPipeline;

namespace Telegram.Bot.Framework.Controller.Params;

/// <summary>
/// 
/// </summary>
internal class ParameterManager(List<BaseParameterGetter> parameterGetters)
{
    /// <summary>
    /// 
    /// </summary>
    private readonly List<BaseParameterGetter> _parameters = parameterGetters;

    /// <summary>
    /// 
    /// </summary>
    private readonly List<BaseParameterGetter> _Copy = [];

    /// <summary>
    /// 
    /// </summary>
    private readonly List<object> _param = [];

    /// <summary>
    /// 
    /// </summary>
    public void Init()
    {
        _Copy.Clear();
        _Copy.AddRange(_parameters);
        _Copy.ForEach(x => x.Init());
    }

    public async Task<EnumReadParam> Read(TelegramUserChatContext context)
    {
        var param = _Copy.FirstOrDefault();
        if (param == null)
            return EnumReadParam.OK;

        EnumReadParam result;
        result = await ParamPipelineStatic.PipelineController.Execute(param.Result,
            new PipelineModel
            {
                ParameterGetter = param,
                ParamList = _param,
                UserChatContext = context,
            });

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public object[] GetParams() => _param.ToArray();
}

