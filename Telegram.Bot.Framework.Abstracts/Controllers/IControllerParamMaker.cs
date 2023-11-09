namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    internal interface IControllerParamMaker
    {
        public IControllerParam Make(Type paramType, IControllerParamSender controllerParamSender);
    }
}
