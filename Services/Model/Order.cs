﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess1;

namespace Services.Model
{
    public class Order
    {
        
        public int OrderID { get; set; }

        
        public string CustomerID { get; set; }

        public int? EmployeeID { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public int? ShipVia { get; set; }

        
        public decimal? Freight { get; set; }

        
        public string ShipName { get; set; }

        
        public string ShipAddress { get; set; }

        
        public string ShipCity { get; set; }

        
        public string ShipRegion { get; set; }

        
        public string ShipPostalCode { get; set; }

        
        public string ShipCountry { get; set; }

        public virtual Customer Customers { get; set; }

        public virtual Employee Employees { get; set; }

       
        public virtual ICollection<Order_Detail>  Order_Details { get; set; }

        public virtual Shippers Shippers { get; set; }
    }
}

