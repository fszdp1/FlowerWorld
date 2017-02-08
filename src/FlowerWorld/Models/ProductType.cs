using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            ProductClass = new HashSet<ProductClass>();
        }

        public int ObjId { get; set; }
        public string ClassifyType { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<ProductClass> ProductClass { get; set; }
    }
}
