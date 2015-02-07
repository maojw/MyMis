using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess.Enum
{
    /// <summary>
    /// 字符串参数的处理进度
    /// </summary>
    public enum SPStep
    {
        // 没开始或者已完成一次字符串参数的拼接。
        NotSet,
        // 拼接时遇到一个单引号结束
        EndWith,
        // 已跳过一次拼接
        Skip		
    }
}
