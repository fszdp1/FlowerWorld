using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class City
    {
        public City()
        {
            Area = new HashSet<Area>();
        }

        public int ObjId { get; set; }
        public string Name { get; set; }
        public int? TheProvince { get; set; }

        public virtual ICollection<Area> Area { get; set; }
        public virtual Province TheProvinceNavigation { get; set; }
    }
}
