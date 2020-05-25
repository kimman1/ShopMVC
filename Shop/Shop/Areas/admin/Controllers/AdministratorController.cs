using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Shop.Areas.admin.Controllers
{
    public class AdministratorController : Controller
    {
        ShopMVCEntities db = new ShopMVCEntities();
        // GET: admin/Administrator
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            bool status = false;
            string username = fc["username"];
            string password = fc["password"];
            var p = (from ad in db.admins where ad.username.Equals(username) && ad.password.Equals(password) select ad);
            foreach (Shop.admin a in p)
            {
                if (a.username != "")
                {
                    ViewBag.Mess = "Login Successful";
                    status = true;
                }
                else
                {
                    ViewBag.Mess = "Login Failed";

                }
            }
            if (status == true)
            {
                return RedirectToAction("Index", "Customers");  
            }
            else
            {
                return View();
            }

        }
    }
}