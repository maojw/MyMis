using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess
{
    public class ConnectionEventArgs : EventArgs
    {
        public DbConnection Connection { get;internal set; }
    }
}
