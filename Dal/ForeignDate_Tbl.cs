//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class ForeignDate_Tbl
    {
        public int ForeignDateId { get; set; }
        public System.DateTime ForeignDate { get; set; }
        public int HebrewDateId { get; set; }
    
        public virtual HebrewDate_Tbl HebrewDate_Tbl { get; set; }
    }
}
