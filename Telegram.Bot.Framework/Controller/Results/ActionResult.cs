using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActionResult : IActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        protected List<IMessageFragment> MessageFragments { get; } =
#if NET8_0_OR_GREATER
            [];
#else
            new List<IMessageFragment>();
#endif

        private string MessageText { get; set; } = string.Empty;

        private readonly Dictionary<FragmentType, Func<TelegramActionContext, IMessageFragment, Task>> combination = new Dictionary<FragmentType, Func<TelegramActionContext, IMessageFragment, Task>>
        {
            [FragmentType.IsText] = async (context, messageFragment) =>
            {
                if (messageFragment.Data == null) return;

                const int maxMessageLength = 4096;
                var bytes = new byte[maxMessageLength];
                var size = messageFragment.Data[0].Read(bytes);
                if (size == maxMessageLength)
                    return;
            },
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ExecuteResultAsync(TelegramActionContext context)
        {
            try
            {
                foreach (var item in MessageFragments)
                {
                    if (item.Data == null)
                        continue;

                    await combination[item.FragmentType](context, item);
                }
            }
            finally
            {
                foreach (var item in MessageFragments)
                {
                    if (item.Data == null)
                        continue;

                    foreach (var data in item.Data)
                        await data.DisposeAsync();
                }
            }
        }
    }
}
