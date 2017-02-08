using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int ObjId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
