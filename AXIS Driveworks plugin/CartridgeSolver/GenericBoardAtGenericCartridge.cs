//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CartridgeSolver
{
    using System;
    using System.Collections.Generic;
    
    public partial class GenericBoardAtGenericCartridge
    {
        public int id { get; set; }
        public int GenericCartridge { get; set; }
        public int GenericBoard { get; set; }
        public int Quantity { get; set; }
    
        public virtual GenericBoardsData GenericBoardsData { get; set; }
        public virtual GenericCartridgesData GenericCartridgesData { get; set; }
    }
}
