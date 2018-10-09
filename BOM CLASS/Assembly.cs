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
    
    public partial class Assembly
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Assembly()
        {
            this.ParentAssemblyAtAssemblies = new HashSet<AssemblyAtAssembly>();
            this.AssemblyAtAssemblies = new HashSet<AssemblyAtAssembly>();
            this.PartAtAssemblies = new HashSet<PartAtAssembly>();
            this.PartRulesFilters = new HashSet<PartRulesFilter>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public Nullable<int> AssemblyTypeID { get; set; }
    
        public virtual AvailableAssemblyType AvailableAssemblyType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssemblyAtAssembly> ParentAssemblyAtAssemblies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssemblyAtAssembly> AssemblyAtAssemblies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartAtAssembly> PartAtAssemblies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartRulesFilter> PartRulesFilters { get; set; }
    }
}