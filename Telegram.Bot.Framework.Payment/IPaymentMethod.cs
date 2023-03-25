using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Payment
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public interface IPaymentMethod
    {
        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        public Task Pay();
    }
}
