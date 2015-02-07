using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess
{
    /// <summary>
    /// 数据库工具类
    /// </summary>
    public static class DbHelper
    {
        #region 对外开发的接口

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteNonQuery(SqlCommand command)
        {
            using (ConnectionScope scope = new ConnectionScope())
            {
                return scope.Current.ExecuteCommand<int>(command, cmd => cmd.ExecuteNonQuery());
            }
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="command">命令</param>
        /// <returns>类型</returns>
        public static T ExecuteScalar<T>(SqlCommand command)
        {
            Func<SqlCommand, T> func = null;
            using (ConnectionScope scope = new ConnectionScope())
            {
                if (func == null)
                {
                    func = cmd => ConvertScalar<T>(cmd.ExecuteScalar());
                }
                return scope.Current.ExecuteCommand<T>(command, func);
            }
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>集合</returns>
        public static DataSet FillDataSet(SqlCommand command)
        {
            using (ConnectionScope scope = new ConnectionScope())
            {
                return scope.Current.ExecuteCommand<DataSet>(command, delegate(SqlCommand cmd)
                {
                    DataSet dataSet = new DataSet();
                    new SqlDataAdapter(cmd).Fill(dataSet);
                    for (int j = 0; j < dataSet.Tables.Count; j++)
                    {
                        dataSet.Tables[j].TableName = "_tb" + j.ToString();
                    }
                    return dataSet;
                });
            }
        }

        /// <summary>
        /// 获取数据表数据
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>表</returns>
        public static DataTable FillDataTable(SqlCommand command)
        {
            using (ConnectionScope scope = new ConnectionScope())
            {
                return scope.Current.ExecuteCommand<DataTable>(command, delegate(SqlCommand cmd)
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataSet set = new DataSet
                        {
                            EnforceConstraints = false
                        };
                        DataTable table = new DataTable("_tb");
                        set.Tables.Add(table);
                        table.Load(reader);
                        return table;
                    }
                });
            }
        }

        /// <summary>
        /// 列集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="command">命令</param>
        /// <returns>类型集合</returns>
        public static List<T> FillScalarList<T>(SqlCommand command)
        {
            Func<SqlCommand, List<T>> func = null;
            using (ConnectionScope scope = new ConnectionScope())
            {
                if (func == null)
                {
                    func = delegate(SqlCommand cmd)
                    {
                        List<T> list = new List<T>();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(ConvertScalar<T>(reader[0]));
                            }
                            return list;
                        }
                    };
                }
                return scope.Current.ExecuteCommand<List<T>>(command, func);
            }
        }

        /// <summary>
        /// 获取列名称
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static string[] GetColumnNames(SqlDataReader reader)
        {
            int fieldCount = reader.FieldCount;
            string[] strArray = new string[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
                strArray[i] = reader.GetName(i);
            }
            return strArray;
        }          

        #endregion

        #region 内部调用的私有方法

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        private static T ConvertScalar<T>(object obj)
        {
            if (obj == null || DBNull.Value.Equals(obj))
            {
                return default(T);
            }

            if (obj is T)
            {
                return (T)obj;
            }

            Type conversionType = typeof(T);

            if (conversionType == typeof(object))
            {
                return (T)obj;
            }

            return (T)Convert.ChangeType(obj, conversionType);
        }

        #endregion
    }
}
