using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Division
    {
        public Division()
        {
            User = new HashSet<User>();
        }

        public int ObjId { get; set; }
        public string Name { get; set; }
        public int? TheArea { get; set; }
        public string StreeName { get; set; }
        public string RoadName { get; set; }
        public string DoorNumber { get; set; }
        public int? ZipCode { get; set; }

        public virtual ICollection<User> User { get; set; }
        public virtual Area TheAreaNavigation { get; set; }
    }
}
