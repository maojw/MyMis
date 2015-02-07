using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMIS.Framework.DataAccess.BaseClass
{
    /// <summary>
    /// IgnoreColumnAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IgnoreColumnAttribute : Attribute
    {
    }
}
