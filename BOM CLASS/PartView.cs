//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BOM_CLASS
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartView
    {
        public int id { get; set; }
        public int PartID { get; set; }
        public int AssemblyID { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> AssemblyTypeID { get; set; }
        public string AssemblyTypeName { get; set; }
        public Nullable<int> PartsID { get; set; }
        public string PartName { get; set; }
        public string Description { get; set; }
        public Nullable<int> PartTypeID { get; set; }
        public string PartTypeName { get; set; }
    }
}
