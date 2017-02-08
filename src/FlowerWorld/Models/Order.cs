using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Order
    {
        public Order()
        {
            CustomerWords = new HashSet<CustomerWords>();
            Receipt = new HashSet<Receipt>();
        }

        public int ObjId { get; set; }
        public int? TheCustomer { get; set; }
        public DateTime? OrderTime { get; set; }
        public int? TheProduct { get; set; }
        public double? Amt { get; set; }
        public int? ThePayment { get; set; }
        public int? TheConsignee { get; set; }
        public int? TheClerk { get; set; }
        public int? TheDeliverer { get; set; }
        public int? OrderState { get; set; }

        public virtual ICollection<CustomerWords> CustomerWords { get; set; }
        public virtual ICollection<Receipt> Receipt { get; set; }
        public virtual User TheClerkNavigation { get; set; }
        public virtual Consignee TheConsigneeNavigation { get; set; }
        public virtual Customer TheCustomerNavigation { get; set; }
        public virtual User TheDelivererNavigation { get; set; }
        public virtual Payment ThePaymentNavigation { get; set; }
        public virtual Product TheProductNavigation { get; set; }
    }
}
