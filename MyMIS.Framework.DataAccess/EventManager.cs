using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace MyMIS.Framework.DataAccess
{
    /// <summary>
    /// EventManager
    /// </summary>
    public static class EventManager
    {
        #region Events

        public static event EventHandler<CommandEventArgs> AfterExecute;
        public static event EventHandler<CommandEventArgs> BeforeExecute;
        public static event EventHandler<ConnectionEventArgs> ConnectionOpened;
        public static event EventHandler<ExceptionEventArgs> OnException;

        #endregion

        #region 方法

        /// <summary>
        /// Fires the after execute.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="data">The data.</param>
        internal static void FireAfterExecute(DbCommand cmd, object data)
        {
            EventHandler<CommandEventArgs> afterExecute = AfterExecute;
            if (afterExecute != null)
            {
                CommandEventArgs e = new CommandEventArgs
                {
                    Command = cmd,
                    UserData = data
                };
                afterExecute(null, e);
            }
        }

        /// <summary>
        /// Fires the before execute.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        internal static object FireBeforeExecute(DbCommand cmd)
        {
            object userData = null;
            EventHandler<CommandEventArgs> beforeExecute = BeforeExecute;
            if (beforeExecute != null)
            {
                CommandEventArgs e = new CommandEventArgs
                {
                    Command = cmd
                };
                beforeExecute(null, e);
                userData = e.UserData;
            }
            return userData;
        }

        /// <summary>
        /// Fires the connection opened.
        /// </summary>
        /// <param name="conn">The connection.</param>
        internal static void FireConnectionOpened(DbConnection conn)
        {
            EventHandler<ConnectionEventArgs> connectionOpened = ConnectionOpened;
            if (connectionOpened != null)
            {
                ConnectionEventArgs e = new ConnectionEventArgs
                {
                    Connection = conn
                };
                connectionOpened(null, e);
            }
        }

        /// <summary>
        /// Fires the on exception.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="data">The data.</param>
        internal static void FireOnException(DbCommand cmd, Exception ex, object data)
        {
            EventHandler<ExceptionEventArgs> onException = OnException;
            if (onException != null)
            {
                ExceptionEventArgs e = new ExceptionEventArgs
                {
                    Command = cmd,
                    Exception = ex,
                    UserData = data
                };
                onException(null, e);
            }
        }
        #endregion
    }
}
