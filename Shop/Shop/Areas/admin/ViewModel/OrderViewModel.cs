using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Areas.admin.ViewModel
{
    public class OrderViewModel
    {
        //public int OrderDetailsID { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public virtual Product Product { get; set; }
        public List<Product> productIDList { get; set; }
        public SelectList productList { get; set; }
    }
}