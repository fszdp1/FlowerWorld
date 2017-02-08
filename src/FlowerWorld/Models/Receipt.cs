using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class Receipt
    {
        public int ObjId { get; set; }
        public int? TheOrder { get; set; }
        public string ReceiptFile { get; set; }

        public virtual Order TheOrderNavigation { get; set; }
    }
}
