using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyMIS.Framework.DataAccess.BaseClass
{
    /// <summary>
    /// 映射
    /// </summary>
    public class DbMapInfo
    {
        public DataColumnAttribute AttrColumn { get; private set; }
        public IgnoreColumnAttribute AttrIgnore { get; private set; }
        public string DbName { get; private set; }
        public string NetName { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMapInfo"/> class.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="netName">Name of the net.</param>
        /// <param name="attrColumn">The attribute column.</param>
        /// <param name="attrIgnore">The attribute ignore.</param>
        /// <param name="prop">The property.</param>
        public DbMapInfo(string dbName, string netName, DataColumnAttribute attrColumn, IgnoreColumnAttribute attrIgnore, PropertyInfo prop)
        {
            this.DbName = dbName;
            this.NetName = netName;
            this.AttrColumn = attrColumn;
            this.PropertyInfo = prop;
            this.AttrIgnore = attrIgnore;
        }
    }
}
