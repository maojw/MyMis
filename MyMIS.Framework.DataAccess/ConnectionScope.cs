using MyMIS.Framework.DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess
{
    /// <summary>
    /// 表示连接与事务的作用域
    /// </summary>
    public sealed class ConnectionScope : IDisposable
    {

        #region 字段

        [ThreadStatic]
        //连接管理
        private static ConnectionManager _connection;
        //连接字符串
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        #endregion

        #region 属性

        internal ConnectionManager Current
        {
            get
            {
                return _connection;
            }
        }

        #endregion

        #region 构造函数

        #endregion

        #region 对外开放的公共接口

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionScope"/> class.
        /// </summary>
        public ConnectionScope()
            : this(TransactionMode.Inherits)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionScope"/> class.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public ConnectionScope(TransactionMode mode)
        {
            this.Init(mode, _connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionScope"/> class.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="System.ArgumentNullException">connectionString</exception>
        /// <exception cref="System.NotSupportedException">内层的ConnectionScope不能再指定连接字符串。</exception>
        public ConnectionScope(TransactionMode mode, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (_connection != null)
            {
                throw new NotSupportedException("内层的ConnectionScope不能再指定连接字符串。");
            }

            this.Init(mode, connectionString);
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">还没有打开数据库连接，无法完成提交请求。</exception>
        public void Commit()
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("还没有打开数据库连接，无法完成提交请求。");
            }
            _connection.Commit();
        }

        /// <summary>
        /// Creates the SQL bulk copy.
        /// </summary>
        /// <param name="copyOptions">The copy options.</param>
        /// <returns></returns>
        public SqlBulkCopy CreateSqlBulkCopy(SqlBulkCopyOptions copyOptions)
        {
            return _connection.CreateSqlBulkCopy(copyOptions);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            if ((_connection != null) && !_connection.PopTransactionMode())
            {
                ForceClose();
            }
        }


        #endregion

        #region 内部调用的私有方法

        /// <summary>
        /// Forces the close.
        /// </summary>
        internal static void ForceClose()
        {
            if (_connection != null)
            {
                try
                {
                    _connection.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _connection = null;
                }
            }
        }

        /// <summary>
        /// Gets the default connection string.
        /// </summary>
        /// <returns></returns>
        internal static string GetDefaultConnectionString()
        {
            return _connectionString;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="connectionString">The connection string.</param>
        private void Init(TransactionMode mode, string connectionString)
        {
            if (_connection == null)
            {
                _connection = new ConnectionManager(connectionString);
            }
            _connection.PushTransactionMode(mode);
        }

        /// <summary>
        /// Rollbacks the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.InvalidOperationException">还没有打开数据库连接，无法完成回滚请求。</exception>
        public void Rollback(string message)
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("还没有打开数据库连接，无法完成回滚请求。");
            }
            _connection.Rollback(message);
        }

        internal static void SetDefaultConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            _connectionString = connectionString;
        }

        #endregion

    }
}
