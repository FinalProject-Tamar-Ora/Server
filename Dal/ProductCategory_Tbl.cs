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
    
    public partial class ProductCategory_Tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductCategory_Tbl()
        {
            this.Product_Tbl = new HashSet<Product_Tbl>();
            this.ProductCategory_Tbl1 = new HashSet<ProductCategory_Tbl>();
        }
    
        public int ProductCategoryId { get; set; }
        public Nullable<int> ParentProductCategoryId { get; set; }
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Tbl> Product_Tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCategory_Tbl> ProductCategory_Tbl1 { get; set; }
        public virtual ProductCategory_Tbl ProductCategory_Tbl2 { get; set; }
    }
}