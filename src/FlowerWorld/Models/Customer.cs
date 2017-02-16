using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlowerWorld.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Consignee = new HashSet<Consignee>();
            Order = new HashSet<Order>();
        }

        public int ObjId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int? TheCustomerType { get; set; }
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }
        [Display(Name = "移动电话")]
        public string MobilePhone { get; set; }
        [Display(Name = "办公电话")]
        public string OfficePhone { get; set; }
        [Display(Name = "家庭电话")]
        public string HomePhone { get; set; }
        [Display(Name = "QQ号码")]
        public string QqNumber { get; set; }
        public DateTime? RegistDate { get; set; }

        public virtual ICollection<Consignee> Consignee { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual CustomerType TheCustomerTypeNavigation { get; set; }
    }
}
