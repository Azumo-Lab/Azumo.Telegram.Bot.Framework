using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Abstract.Sessions
{
    /// <summary>
    /// Session操作保存
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// SessionID
        /// </summary>
        public string SessionID { get; }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sessionKey">保存的Key</param>
        /// <param name="data">保存的数据</param>
        public void Save(object sessionKey, byte[] data);

        /// <summary>
        /// 获得数据
        /// </summary>
        /// <param name="sessionKey">保存的Key</param>
        /// <returns>返回保存数据</returns>
        public byte[] Get(object sessionKey);

        /// <summary>
        /// 清除某个数据
        /// </summary>
        /// <param name="sessionKey">保存的Key</param>
        public void Remove(object sessionKey);
    }
}
