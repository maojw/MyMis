using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess.Interface
{
    /// <summary>
    /// 数据库执行接口
    /// </summary>
    public interface IDbExecute
    {
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <returns>影响的行数</returns>
        int ExecuteNonQuery();

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <returns>实体</returns>
        T ExecuteScalar<T>();

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <returns>数据集</returns>
        DataSet FillDataSet();

        /// <summary>
        /// 查询数据表
        /// </summary>
        /// <returns>数据表</returns>
        DataTable FillDataTable();

        /// <summary>
        /// 查询单列集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>单列集合</returns>
        List<T> FillScalarList<T>();

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <returns>实体集合</returns>
        List<T> ToList<T>() where T : class,new();

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>单个实体</returns>
        T ToSingle<T>() where T : class,new();

    }
}
