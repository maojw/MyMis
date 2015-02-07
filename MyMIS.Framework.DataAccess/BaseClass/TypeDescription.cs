using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess.BaseClass
{
    /// <summary>
    /// 类型描述
    /// </summary>
    public class TypeDescription
    {
        public Func<int, object[], object> ExecuteFunc { get; set; }
        public Dictionary<string, DbMapInfo> MemberDict{get;set;}
    }
}
