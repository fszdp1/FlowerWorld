using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class CustomerType
    {
        public CustomerType()
        {
            Customer = new HashSet<Customer>();
            PriceList = new HashSet<PriceList>();
        }

        public int ObjId { get; set; }
        public string TypeName { get; set; }
        public double? MinSpending { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<PriceList> PriceList { get; set; }
    }
}
