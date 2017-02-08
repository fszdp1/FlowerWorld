using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Area
    {
        public Area()
        {
            Consignee = new HashSet<Consignee>();
            Division = new HashSet<Division>();
        }

        public int ObjId { get; set; }
        public string Name { get; set; }
        public int? TheCity { get; set; }

        public virtual ICollection<Consignee> Consignee { get; set; }
        public virtual ICollection<Division> Division { get; set; }
        public virtual City TheCityNavigation { get; set; }
    }
}
