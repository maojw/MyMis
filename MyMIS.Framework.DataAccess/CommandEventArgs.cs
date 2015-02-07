using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess
{
    /// <summary>
    /// CommandEventArgs
    /// </summary>
    public class CommandEventArgs
    {
        public DbCommand Command { get; internal set; }
        public object UserData { get; set; }
    }
}
