using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess.BaseClass
{
    /// <summary>
    /// TypeDescriptionCache
    /// </summary>
    internal class TypeDescriptionCache
    {
        #region 字段
        private static BindingFlags s_flag = (BindingFlags.Public | BindingFlags.Instance);
        private static Hashtable s_typeInfoDict = Hashtable.Synchronized(new Hashtable(0x800));
        #endregion

        #region 方法

        /// <summary>
        /// Gets the type discription.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static TypeDescription GetTypeDiscription(Type type)
        {
            TypeDescription description = s_typeInfoDict[type.FullName] as TypeDescription;
            if (description == null)
            {
                PropertyInfo[] properties = type.GetProperties(s_flag);
                int length = properties.Length;
                Dictionary<string, DbMapInfo> dictionary = new Dictionary<string, DbMapInfo>(length, StringComparer.OrdinalIgnoreCase);
                foreach (PropertyInfo info in properties)
                {
                    DbMapInfo info2 = null;
                    DataColumnAttribute myAttribute = info.GetMyAttribute<DataColumnAttribute>();
                    IgnoreColumnAttribute attrIgnore = info.GetMyAttribute<IgnoreColumnAttribute>();
                    if (myAttribute != null)
                    {
                        info2 = new DbMapInfo(string.IsNullOrEmpty(myAttribute.Alias) ? info.Name : myAttribute.Alias, info.Name, myAttribute, attrIgnore, info);
                    }
                    else
                    {
                        info2 = new DbMapInfo(info.Name, info.Name, null, attrIgnore, info);
                    }
                    dictionary[info2.DbName] = info2;
                }
                description = new TypeDescription
                {
                    MemberDict = dictionary
                };
                s_typeInfoDict[type.FullName] = description;
            }
            return description;
        }

        /// <summary>
        /// Saves the complie result.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        public static void SaveComplieResult(Type type, TypeDescription description)
        {
            s_typeInfoDict[type.FullName] = description;
        }

        #endregion

    }
}
