using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.PaymentMethod;

/// <summary>
/// 订单管理
/// </summary>
public interface IOrderManager
{
    /// <summary>
    /// 创建一个订单
    /// </summary>
    /// <returns></returns>
    public IOrder<T> CreateOrder<T>(T info) where T : class;

    public IOrder GetOrderInfo(TelegramUserChatContext telegramUserChatContext);
}
