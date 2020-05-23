using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Areas.admin.ViewModel
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int InStock { get; set; }
        public int CategoryID { get; set; }
        public virtual Category category { get; set; }
    }
}