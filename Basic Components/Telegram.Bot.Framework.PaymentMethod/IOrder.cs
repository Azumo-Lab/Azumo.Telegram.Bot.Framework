namespace Telegram.Bot.Framework.PaymentMethod;

/// <summary>
/// 一个订单信息
/// </summary>
public interface IOrder
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public object OrderInfo { get; }
    /// <summary>
    /// 完成订单
    /// </summary>
    /// <returns></returns>
    public Task<bool> CompleteOrder();

    /// <summary>
    /// 订单关闭
    /// </summary>
    /// <returns></returns>
    public Task<bool> CloseOrder();

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <returns></returns>
    public Task<bool> CancelOrder();
}

public interface IOrder<TInfo> : IOrder where TInfo : class
{
    public new TInfo OrderInfo { get; set; }
}
