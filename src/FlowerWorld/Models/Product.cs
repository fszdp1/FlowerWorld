using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Product
    {
        public Product()
        {
            Order = new HashSet<Order>();
            PriceList = new HashSet<PriceList>();
            ProductClass = new HashSet<ProductClass>();
        }

        public int ObjId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Feature { get; set; }
        public string Description { get; set; }
        public string Meaning { get; set; }
        public double? Price { get; set; }
        public string SmallImg { get; set; }
        public string BigImg { get; set; }
        public int? ProductState { get; set; }

        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<PriceList> PriceList { get; set; }
        public virtual ICollection<ProductClass> ProductClass { get; set; }
    }
}
