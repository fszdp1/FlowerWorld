using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Consignee
    {
        public Consignee()
        {
            Order = new HashSet<Order>();
        }

        public int ObjId { get; set; }
        public string Name { get; set; }
        public int? TheArea { get; set; }
        public int? TheCustomer { get; set; }
        public string StreetName { get; set; }
        public string RoadName { get; set; }
        public string DoorNumber { get; set; }
        public int? ZipCode { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string OfficePhone { get; set; }
        public string HomePhone { get; set; }
        public string QqNumber { get; set; }

        public virtual ICollection<Order> Order { get; set; }
        public virtual Area TheAreaNavigation { get; set; }
        public virtual Customer TheCustomerNavigation { get; set; }
    }
}
