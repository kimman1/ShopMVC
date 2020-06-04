using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Areas.admin.Controllers
{
    public class ProblemController : Controller
    {
        // GET: admin/Problem
        public ActionResult Problem()
        {
            ViewBag.orderid = TempData["orderID"];
            ViewBag.ProductIDerror = TempData["status"];
            return View();
        }
    }
}