using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Province
    {
        public Province()
        {
            City = new HashSet<City>();
        }

        public int ObjId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
