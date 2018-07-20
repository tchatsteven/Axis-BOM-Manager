//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BOM_MANAGER
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartRule
    {
        public int id { get; set; }
        public int PartID { get; set; }
        public string PartName { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ParameterID { get; set; }
        public string ParameterName { get; set; }
        public Nullable<int> PACAF_ID { get; set; }
        public Nullable<int> FirstFilterDependencyID { get; set; }
        public string FirstFilterDependencyName { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual FilterBehavior FilterBehavior { get; set; }
        public virtual Part Part { get; set; }
        public virtual ParameterAtCategoryAtFixture ParameterAtCategoryAtFixture { get; set; }
    }
}