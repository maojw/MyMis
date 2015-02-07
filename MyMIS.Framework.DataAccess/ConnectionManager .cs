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
    /// 连接管理类
    /// </summary>
    internal class ConnectionManager : IDisposable
    {

        #region 字段

        private SqlConnection _connection;
        private string _connectionString;
        private bool _enableTransaction;
        private Stack<TransactionMode> _transactionMode;
        private SqlTransaction _transaction;

        #endregion

        #region 属性

        public SqlConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="System.ArgumentNullException">connectionString</exception>
        public ConnectionManager(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            this._connectionString = connectionString;
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            TransactionMode tm = this._transactionMode.Peek();

            if (tm != TransactionMode.Required || this._connection != null)
            {
                if (this._transaction == null)
                {
                    throw new InvalidOperationException("当前作用域不支持事物操作。");
                }

                if (tm != TransactionMode.Required)
                {
                    throw new InvalidOperationException("未在构造函数中指定TransactionMode.Required参数,不能调用Commit方法。");
                }

                tm = this._transactionMode.Pop();

                try
                {
                    if (this._enableTransaction && tm == TransactionMode.Required && !this._transactionMode.Contains(TransactionMode.Required))
                    {
                        this._transaction.Commit();
                    }
                }
                finally
                {
                    this._transactionMode.Push(tm);
                }
            }

        }

        /// <summary>
        /// 创建SqlBulkCopy
        /// </summary>
        /// <param name="copyOptions">The copy options.</param>
        /// <returns></returns>
        public SqlBulkCopy CreateSqlBulkCopy(SqlBulkCopyOptions copyOptions)
        {
            if (this._connection == null)
            {
                this._connection = new SqlConnection(this._connectionString);
                this._connection.Open();
                EventManager.FireConnectionOpened(this._connection);
            }

            if (this._enableTransaction && this._transaction == null)
            {
                this._transaction = this._connection.BeginTransaction();
            }

            return new SqlBulkCopy(this._connection, copyOptions, this._transaction);

        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this._transactionMode.Clear();

            if (this._connection != null)
            {
                this._connection.Dispose();
                this._connection = null;
            }

            if (this._transaction != null)
            {
                this._transaction.Dispose();
                this._transaction = null;
            }

        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">command</exception>
        public T ExecuteCommand<T>(SqlCommand command, Func<SqlCommand, T> func)
        {
            T local2;
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            object result = MockResult.GetResult();
            if (result != null)
            {
                return (T)result;
            }
            if (this._connection == null)
            {
                this._connection = new SqlConnection(this._connectionString);
                this._connection.Open();
                EventManager.FireConnectionOpened(this._connection);
            }
            if (this._enableTransaction && this._transaction == null)
            {
                this._transaction = this._connection.BeginTransaction();
            }
            command.Connection = this._connection;
            if (this._transaction != null)
            {
                command.Transaction = this._transaction;
            }
            object data = EventManager.FireBeforeExecute(command);
            try
            {
                T local = func(command);
                EventManager.FireAfterExecute(command, data);
                local2 = local;
            }
            catch (Exception exception)
            {
                EventManager.FireOnException(command, exception, data);
                throw;
            }
            finally
            {
                command.Connection = null;
                command.Transaction = null;
            }
            return local2;
        }

        /// <summary>
        /// Pops the transaction mode.
        /// </summary>
        /// <returns></returns>
        internal bool PopTransactionMode()
        {
            TransactionMode mode = this._transactionMode.Pop();
            if ((this._enableTransaction && (mode == TransactionMode.Required)) && !this._transactionMode.Contains(TransactionMode.Required))
            {
                this._enableTransaction = false;
                if (this._transaction != null)
                {
                    this._transaction.Dispose();
                    this._transaction = null;
                }
            }
            return (this._transactionMode.Count != 0);
        }

        /// <summary>
        /// Pushes the transaction mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        internal void PushTransactionMode(TransactionMode mode)
        {
            this._transactionMode.Push(mode);
            if ((mode == TransactionMode.Required) && !this._enableTransaction)
            {
                this._enableTransaction = true;
            }
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="message"></param>
        public void Rollback(string message)
        {
            if (this._transaction == null)
            {
                throw new InvalidOperationException("当前的作用域不支持事务操作。");
            }
        }

        #endregion
    }
}
