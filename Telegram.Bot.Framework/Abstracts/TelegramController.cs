using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.Abstracts
{
    public abstract class TelegramController
    {
        protected TGChat Chat { get; private set; }

        internal async Task ControllerInvokeAsync(TGChat Chat, Func<TelegramController, object[], Task> func, IControllerParamManager controllerParamManager)
        {
            this.Chat = Chat;

            try
            {
                if (func(this, controllerParamManager.GetObjects() ?? Array.Empty<object>()) is Task task)
                    await task;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                controllerParamManager.Clear();
            }
        }
    }
}
