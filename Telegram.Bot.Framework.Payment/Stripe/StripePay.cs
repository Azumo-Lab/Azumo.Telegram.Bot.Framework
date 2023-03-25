using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Payment.Stripe
{
    public class StripePay : IPaymentMethod
    {
        public Task Pay()
        {
            return Task.CompletedTask;
        }
    }
}
