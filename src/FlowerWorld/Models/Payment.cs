using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Order = new HashSet<Order>();
        }

        public int ObjId { get; set; }
        public double? Amount { get; set; }
        public int? ThePaymentType { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public DateTime? TransTime { get; set; }
        public string TransNo { get; set; }
        public int? PaymentState { get; set; }

        public virtual ICollection<Order> Order { get; set; }
        public virtual PaymentType ThePaymentTypeNavigation { get; set; }
    }
}
