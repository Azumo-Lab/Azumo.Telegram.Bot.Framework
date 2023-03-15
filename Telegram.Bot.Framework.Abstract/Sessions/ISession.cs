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
        /// 保存
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="data"></param>
        public void Save(object sessionKey, byte[] data);

        /// <summary>
        /// 获得
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public byte[] Get(object sessionKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Remove(object sessionKey);
    }
}
