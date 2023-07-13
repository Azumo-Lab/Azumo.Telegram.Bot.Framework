using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOT
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITimedTask
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public Task Exec();
    }
}
