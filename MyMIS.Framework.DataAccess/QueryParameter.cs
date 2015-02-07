using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public sealed class QueryParameter
    {
        #region 字段

        private object _val;

        #endregion

        #region 属性

        public object Value
        {
            get
            {
                return this._val;
            }
        }


        #endregion

        #region 方法

        public QueryParameter(object val)
        {
            this._val = val;
        }

        public static explicit operator QueryParameter(string value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(bool value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(byte value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(DateTime value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(DBNull value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(decimal value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(double value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(Guid value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(short value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(int value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(long value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(float value)
        {
            return new QueryParameter(value);
        }

        public static implicit operator QueryParameter(byte[] value)
        {
            return new QueryParameter(value);
        }


        #endregion

    }
}
