using MyMIS.Framework.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess
{
    /// <summary>
    /// 表示存SQL查询调用的封装
    /// CPQuery使用参数化查询SQL,可以通过匿名对象、SqlParameter数组的方式添加参数
    /// </summary>
    public sealed class CPQuery : IDbExecute
    {
        #region 字段

        private SqlCommand _command = new SqlCommand();
        private int _count;
        private StringBuilder _sb = new StringBuilder(512);

        #endregion

        #region 属性
        /// <summary>
        /// 获取当前CPQuery内部的SqlCommand对象
        /// </summary>
        public SqlCommand Command
        {
            get { return _command; }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <returns></returns>
        internal SqlCommand GetCommand()
        {
            _command.CommandText = _sb.ToString();
            return _command;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="CPQuery"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        internal CPQuery(string text)
        {
            this.AddSqlText(text);
        }

        #endregion

        #region 对外开放的公共接口

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _sb.ToString();
        }

        /// <summary>
        /// 通过参数化SQL、匿名对象的方式,创建CPQuery对象实例
        /// </summary>
        /// <returns></returns>
        public static CPQuery Create()
        {
            return new CPQuery(null);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <returns>
        /// 影响的行数
        /// </returns>
        public int ExecuteNonQuery()
        {
            return DbHelper.ExecuteNonQuery(this.GetCommand());
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <returns>
        /// 实体
        /// </returns>
        public T ExecuteScalar<T>()
        {
            return DbHelper.ExecuteScalar<T>(this.GetCommand());
        }

        /// <summary>
        /// Fills the data set.
        /// </summary>
        /// <returns></returns>
        public DataSet FillDataSet()
        {
            return DbHelper.FillDataSet(this.GetCommand());
        }

        /// <summary>
        /// 查询数据表
        /// </summary>
        /// <returns>
        /// 数据表
        /// </returns>
        public DataTable FillDataTable()
        {
            return DbHelper.FillDataTable(this.GetCommand());
        }

        /// <summary>
        /// 查询单列集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>
        /// 单列集合
        /// </returns>
        public List<T> FillScalarList<T>()
        {
            return DbHelper.FillScalarList<T>(this.GetCommand());
        }

        /// <summary>
        /// Froms the specified parameterized SQL.
        /// </summary>
        /// <param name="parameterizedSQL">The parameterized SQL.</param>
        /// <returns></returns>
        public static CPQuery From(string parameterizedSQL)
        {
            return From(parameterizedSQL, (SqlParameter[])null);
        }

        /// <summary>
        /// Froms the specified parameterized SQL.
        /// </summary>
        /// <param name="parameterizedSQL">The parameterized SQL.</param>
        /// <param name="argsObject">The arguments object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">parameterizedSQL</exception>
        /// <exception cref="System.InvalidOperationException">不支持在IN条件中使用DateTime类型</exception>
        public static CPQuery From(string parameterizedSQL, object argsObject)
        {
            if (string.IsNullOrEmpty(parameterizedSQL))
            {
                throw new ArgumentNullException("parameterizedSQL");
            }
            CPQuery query = new CPQuery(parameterizedSQL);
            if (argsObject != null)
            {
                foreach (PropertyInfo info in argsObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    object obj2 = info.FastGetValue(argsObject);
                    string parameterName = "@" + info.Name;
                    if ((obj2 == null) || (obj2 == DBNull.Value))
                    {
                        query._command.Parameters.AddWithValue(parameterName, DBNull.Value);
                    }
                    else if (obj2 is ICollection)
                    {
                        StringBuilder builder = new StringBuilder(0x80);
                        builder.Append("(");
                        bool flag = true;
                        foreach (object obj3 in obj2 as ICollection)
                        {
                            string str2 = null;
                            if ((obj3 is string) || (obj3 is Guid))
                            {
                                str2 = "N'" + obj3.ToString().Replace("'", "''") + "'";
                            }
                            else
                            {
                                if (obj3 is DateTime)
                                {
                                    throw new InvalidOperationException("不支持在IN条件中使用DateTime类型");
                                }
                                if (obj3 is Guid)
                                {
                                    str2 = "'" + obj3.ToString() + "'";
                                }
                                else
                                {
                                    str2 = obj3.ToString();
                                }
                            }
                            if (flag)
                            {
                                builder.Append(str2);
                                flag = false;
                            }
                            else
                            {
                                builder.AppendFormat(",{0}", str2);
                            }
                        }
                        if (builder.Length == 1)
                        {
                            builder.Append("NULL");
                        }
                        builder.Append(")");
                        string newValue = builder.ToString();
                        query._sb.Replace(parameterName, newValue);
                    }
                    else
                    {
                        SqlParameter parameter = obj2 as SqlParameter;
                        if (parameter != null)
                        {
                            query._command.Parameters.Add(parameter);
                        }
                        else
                        {
                            query._command.Parameters.AddWithValue(parameterName, obj2);
                        }
                    }
                }
            }
            return query;
        }

        /// <summary>
        /// Froms the specified parameterized SQL.
        /// </summary>
        /// <param name="parameterizedSQL">The parameterized SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static CPQuery From(string parameterizedSQL, params SqlParameter[] parameters)
        {
            CPQuery query = new CPQuery(parameterizedSQL);
            if (parameters != null)
            {
                query._command.Parameters.AddRange(parameters);
            }
            return query;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CPQuery operator +(CPQuery query, QueryParameter p)
        {
            query.AddParameter(p);
            return query;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CPQuery operator +(CPQuery query, SqlParameter p)
        {
            query.AddSqlText(p.ParameterName);
            query._command.Parameters.Add(p);
            return query;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CPQuery operator +(CPQuery query, string s)
        {
            query.AddSqlText(s);
            return query;
        }

        /// <summary>
        /// To the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> ToList<T>() where T : class, new()
        {
            return DbHelper.ToList<T>(this.GetCommand());
        }

        public T ToSingle<T>() where T : class, new()
        {
            return DbHelper.ToSingle<T>(this.GetCommand());
        }


        #endregion

        #region 内部调用的私有方法

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="p">The p.</param>
        private void AddParameter(QueryParameter p)
        {
            string name = "@p" + (this._count++).ToString();
            _sb.Append(name);
            _command.Parameters.AddWithValue(name, p.Value);
        }

        /// <summary>
        /// Adds the SQL text.
        /// </summary>
        /// <param name="text">The text.</param>
        private void AddSqlText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this._sb.Append(text);
            }
        }

        internal static CPQuery Format(string format, params object[] parameters)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }
            if ((parameters == null) || (parameters.Length == 0))
            {
                return format.AsCPQuery();
            }
            string[] strArray = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                strArray[i] = "@p" + i.ToString();
            }
            CPQuery query = string.Format(format, (object[])strArray).AsCPQuery();
            for (int j = 0; j < parameters.Length; j++)
            {
                object obj2 = parameters[j];
                if ((obj2 == null) || (obj2 == DBNull.Value))
                {
                    query._command.Parameters.AddWithValue(strArray[j], DBNull.Value);
                }
                else
                {
                    query._command.Parameters.AddWithValue(strArray[j], obj2);
                }
            }
            return query;
        }

        #endregion


    }
}
