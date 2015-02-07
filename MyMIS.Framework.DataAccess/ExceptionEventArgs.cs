using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace MyMIS.Framework.DataAccess
{
    /// <summary>
    /// ExceptionEventArgs
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        public DbCommand Command { get; internal set; }
        public Exception Exception { get; internal set; }
        public object UserData { get; internal set; }
    }
}
