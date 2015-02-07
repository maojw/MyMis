using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMIS.Framework.DataAccess.BaseClass
{
    /// <summary>
    /// 数据列属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DataColumnAttribute
    {
        //别名
        public string Alias { get; set; }

        //默认值
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataColumnAttribute"/> is identity.
        /// </summary>
        /// <value>
        ///   <c>true</c> if identity; otherwise, <c>false</c>.
        /// </value>
        public bool Identity { get; set; }
        
        //为空
        public bool IsNullable { get; set; }

        //主键
        public bool PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [seq unique identifier].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [seq unique identifier]; otherwise, <c>false</c>.
        /// </value>
        public bool SeqGuid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [time stamp].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [time stamp]; otherwise, <c>false</c>.
        /// </value>
        public bool TimeStamp { get; set; }
    }
}
