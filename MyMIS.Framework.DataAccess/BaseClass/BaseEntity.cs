using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MyMIS.Framework.DataAccess.Enum;

namespace MyMIS.Framework.DataAccess.BaseClass
{
    /// <summary>
    /// 数据实体基类
    /// </summary>
    [Serializable]
    public abstract class BaseEntity
    {
        #region 字段

        private List<string> _changedProperties;
        private Func<CPQuery, bool> _funcBefore;
        internal BaseEntity bakObject;

        #endregion

        #region 属性

        internal Func<CPQuery, bool> FuncBefore
        {
            get
            {
                return this._funcBefore;
            }
        }

        #endregion
                
        #region 构造函数

        protected BaseEntity()
        { 
        }

        #endregion

        #region 方法

        public string[] ChangedProperties()
        {
            if (this._changedProperties == null)
            {
                return new string[0];
            }
            return this._changedProperties.ToArray();
        }

        /// <summary>
        /// 将数据实体对应的记录从数据库表中删除。
        /// </summary>
        /// <returns></returns>
        public virtual int Delete()
        {
            CPQuery cPQuery = this.GetCPQuery(4, new object[] { this });
            if ((this._funcBefore != null) && !this._funcBefore(cPQuery))
            {
                return -1;
            }
            return cPQuery.ExecuteNonQuery();
        }

        public virtual int Delete(ConcurrencyMode concurrencyMode)
        {
            int flag = (concurrencyMode == ConcurrencyMode.TimeStamp) ? 5 : 6;
            CPQuery cPQuery = this.GetCPQuery(flag, new object[] { this });
            if ((this._funcBefore != null) && !this._funcBefore(cPQuery))
            {
                return -1;
            }
            int num2 = cPQuery.ExecuteNonQuery();
            if (num2 == 0)
            {
                throw new OptimisticConcurrencyException("并发操作失败，本次操作没有删除任何记录，请确认当前数据行没有被其他用户更新或删除。");
            }
            return num2;
        }

        internal CPQuery GetCPQuery(int flag, params object[] parameters)
        {
            CPQuery query;
            TypeDescription typeDiscription = TypeDescriptionCache.GetTypeDiscription(base.GetType());
            if (typeDiscription.ExecuteFunc == null)
            {
                throw GetNonStandardExecption(base.GetType());
            }
            try
            {
                query = typeDiscription.ExecuteFunc(flag, parameters) as CPQuery;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return query;
        }

        private static T GetDefaultValue<T>()
        {
            return default(T);
        }

        internal static Exception GetNonStandardExecption(Type entryType)
        {
            return new NonStandardExecption(string.Format("类型 {0} 不能执行指定的操作，因为它的定义不符合规范。请确认已将此类型定义在 .Entity.dll 结尾的程序集中，且不是嵌套类，并已提供无参的构造函数。", entryType.FullName));
        }

        public void HookExecute(Func<CPQuery, bool> func)
        {
            if (this._funcBefore != null)
            {
                throw new InvalidOperationException("在一个对象实例中,BeforeExecute方法只能调用一次。");
            }
            this._funcBefore = func;
        }

        public virtual int Insert()
        {
            CPQuery cPQuery = this.GetCPQuery(3, new object[] { this });
            if (cPQuery == null)
            {
                throw new InvalidOperationException("传入对象不能生成有效的SQL语句。");
            }
            if ((this._funcBefore != null) && !this._funcBefore(cPQuery))
            {
                return -1;
            }
            return cPQuery.ExecuteNonQuery();
        }

        public void SetPropertyDefaultValue(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            PropertyInfo property = base.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new ArgumentOutOfRangeException(string.Format("指定的属性名 {0} 不能匹配任何属性。", propertyName));
            }
            if (!property.PropertyType.IsValueType)
            {
                throw new InvalidOperationException(string.Format("指定的属性名 {0} 不是值类型，不需要这个调用。", propertyName));
            }
            object obj2 = Activator.CreateInstance(property.PropertyType);
            this.SetValue(propertyName, obj2);
        }

        public void SetValue(string propertyName, object value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            PropertyInfo property = base.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("指定的属性名 {0} 不能匹配任何属性。", propertyName));
            }
            if (!property.CanWrite)
            {
                throw new InvalidOperationException(string.Format("指定的属性名 {0} 不可写。", propertyName));
            }
            if (this._changedProperties == null)
            {
                this._changedProperties = new List<string>();
            }
            this._changedProperties.Add(propertyName);
            property.SetValue(this, value, null);
        }

        public void TrackChange()
        {
            TypeDescription typeDiscription = TypeDescriptionCache.GetTypeDiscription(base.GetType());
            if (typeDiscription.ExecuteFunc == null)
            {
                throw GetNonStandardExecption(base.GetType());
            }
            this.bakObject = typeDiscription.ExecuteFunc(13, new object[] { this }) as BaseEntity;
        }

        public virtual int Update()
        {
            CPQuery cPQuery = this.GetCPQuery(7, new object[] { this, this.bakObject });
            if (cPQuery == null)
            {
                return 0;
            }
            if ((this._funcBefore != null) && !this._funcBefore(cPQuery))
            {
                return -1;
            }
            return cPQuery.ExecuteNonQuery();
        }

        public virtual int Update(BaseEntity original, ConcurrencyMode concurrencyMode)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            if ((concurrencyMode == ConcurrencyMode.OriginalValue) && object.ReferenceEquals(this, original))
            {
                throw new ArgumentException("用于并发检测的原始对象不能是当前对象。");
            }
            int flag = (concurrencyMode == ConcurrencyMode.TimeStamp) ? 8 : 9;
            CPQuery cPQuery = this.GetCPQuery(flag, new object[] { this, original, this.bakObject });
            if (cPQuery == null)
            {
                return 0;
            }
            if ((this._funcBefore != null) && !this._funcBefore(cPQuery))
            {
                return -1;
            }
            int num2 = cPQuery.ExecuteNonQuery();
            if (num2 == 0)
            {
                throw new Exception("并发操作失败，本次操作没有更新任何记录，请确认当前数据行没有被其他用户更新或删除。");
            }
            return num2;
        }



        #endregion

    }
}
