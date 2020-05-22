using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class ProductsController : Controller
    {
        ShopMVCEntities db = new ShopMVCEntities();
        // GET: Products
        public ActionResult Index()
        {
            var p = db.Products.Select(s => s).ToList();

            return View(p);
        }
    }
}