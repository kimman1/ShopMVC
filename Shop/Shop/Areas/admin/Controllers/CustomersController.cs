using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop;

namespace Shop.Areas.admin.Controllers
{
    public class CustomersController : Controller
    {
        private ShopMVCEntities db = new ShopMVCEntities();
        int cusOrderID = -1;
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: admin/Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: admin/Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Customer customer)
        {
            
            if (ModelState.IsValid)
            {
                int phonenumber = -1;
                if (customer.Phone == null)
                {
                    customer.Phone = "0";
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (int.TryParse(customer.Phone.ToString(), out phonenumber))
                    {
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.status = "Vui lòng kiểm tra lại số điện thoại hoặc bỏ trống";
                    }
                }
              
                
            }

            return View(customer);
        }

        // GET: admin/Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: admin/Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,Name,Address,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                int phonenumber = 0;
                if (customer.Phone == null)
                {
                    customer.Phone = "0";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (int.TryParse(customer.Phone.ToString(), out phonenumber))
                    {

                        db.Entry(customer).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.status = "Vui lòng kiểm tra SĐT hoặc để trống";
                    }
                }
                   
                
              
            }
            return View(customer);
        }

        // GET: admin/Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            ObjectParameter returnValue = new ObjectParameter("outputresult", typeof(int));
            db.deleteCustomer(id, returnValue);
            if (Convert.ToInt32(returnValue.Value) == 0)
            {
                ViewBag.error = "Xóa thất bại. Kiểm tra Orders";
                return View(customer);  
            }
            else
            {
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            
        }
        public ActionResult CreateOrder()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateOrder(int id, Order oder)
        {
            if (id == -1)
            {
                return View();
            }
            else
            {
                
                oder.CustomerID = id;
                oder.TongTien = 0 + "";
                oder.OrderDate = DateTime.Now;
                db.Orders.Add(oder);
                db.SaveChanges();
                return RedirectToAction("CusOrder", new { id = id });
            }
           
        }
        public ActionResult CusOrder(int id)
        {
           
            var p = db.Orders.Where(s => s.CustomerID == id).ToList();
            ViewBag.cusID = id;
            return View(p);
        }
        public ActionResult OrdersDetails(int id)
        {
            return RedirectToAction("Details", "Orders", new { id = id });
        }
        public ActionResult RedirectToOrder()
        {
            return RedirectToAction("Index","Orders");
        }
        public ActionResult RedirectToProduct()
        {
            return RedirectToAction("Index","Products");
        }
        public ActionResult RedirectToCategories()
        {
            return RedirectToAction("Index", "Categories");
        }
        public ActionResult RedirectToCustomer()
        {
            return RedirectToAction("Index", "Customers");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
