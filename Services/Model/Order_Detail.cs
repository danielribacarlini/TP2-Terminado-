using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess1;

namespace Services.Model
{
    public class Order_Detail
    {
        public int OrderID { get; set; }
        
        public int ProductID { get; set; }

        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public virtual Order Orders { get; set; }

        public virtual Product Products { get; set; }

    }
}
