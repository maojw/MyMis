//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyMIS.Map.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AppFunction
    {
        public System.Guid AppFunctionGUID { get; set; }
        public Nullable<System.Guid> BusinessAppGUID { get; set; }
        public string FunctionName { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionShortCode { get; set; }
        public Nullable<byte> IsDisabled { get; set; }
        public string DisabledReason { get; set; }
        public string FunctionUrl { get; set; }
        public string FunctionIcon { get; set; }
        public string IconClass { get; set; }
        public string IconClssUrl { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string Remark { get; set; }
    }
}
