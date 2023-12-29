using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.PaymentMethod
{
    public interface IOrder
    {
        public void CreateOrder();
    }
}
