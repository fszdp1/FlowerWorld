using System;
using System.Collections.Generic;

namespace FlowerWorld.Models
{
    public partial class CustomerWords
    {
        public int ObjId { get; set; }
        public int? TheOrder { get; set; }
        public string Words { get; set; }

        public virtual Order TheOrderNavigation { get; set; }
    }
}
