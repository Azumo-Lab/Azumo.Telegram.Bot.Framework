using System.ComponentModel.DataAnnotations;

namespace MyChannel.DataBaseContext.DBModels
{
    internal class DBBase
    {
        /// <summary>
        /// KEY
        /// </summary>
        [Key]
        public string Key { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否等待更新
        /// </summary>
        public bool WaitingForUpdate { get; set; }
    }
}
