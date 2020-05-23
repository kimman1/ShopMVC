using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Shop;
using Shop.Areas.admin.ViewModel;

namespace Shop.Areas.admin.Controllers
{
    public class OrdersController : Controller
    {
        private ShopMVCEntities db = new ShopMVCEntities();

        // GET: admin/Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: admin/Orders/Details/5
        /* public ActionResult Details(int? id)
         {
             if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             Order order = db.Orders.Find(id);
             if (order == null)
             {
                 return HttpNotFound();
             }
             return View(order);
         }*/
        public ActionResult Details(int? id)
        {
            List<OrdersDetail> p = db.OrdersDetails.Where(m => m.OrderID == id).ToList();
            List<OrderViewModel> viewmodel = new List<OrderViewModel>();


            foreach (OrdersDetail odd in p)
            {
                OrderViewModel od = new OrderViewModel();
                od.OrderDetailsID = odd.OrdersDetailID;
                od.OrderID = odd.OrderID;
                od.Price = odd.Price.ToString(); ;
                od.Product = odd.Product;
                od.Quantity = (int)odd.Quantity;
                viewmodel.Add(od);
            }
            return View(viewmodel);
        }

        // GET: admin/Orders/Create
        public ActionResult Create()
        {
            /* var p = db.Products.Select(s => s).ToList();
             OrderViewModel ovm = new OrderViewModel();
             ovm.productIDList = p;*/
            List<Customer> listCustomer = db.Customers.ToList();
            SelectList customerlist = new SelectList(listCustomer,"CustomerID","Name");
            ViewBag.CustomerList = customerlist;
            return View();
        }

        // POST: admin/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                string dropdownID = fc["DropDownCustomer"].ToString();
                order.CustomerID = int.Parse(dropdownID);
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }
        /*
        public ActionResult Create(OrderViewModel ovm, int cusID, int id)
        {
            OrdersDetail oDetails = new OrdersDetail();
            oDetails.OrderID = id;
            oDetails.Quantity = ovm.Quantity;
            oDetails.Price =  decimal.Parse(ovm.Price);
            oDetails.ProductID = ovm.Product.ProductID;
            Order order = new Order();
            order.CustomerID = cusID;
            db.Orders.Add(order);
            db.SaveChanges();
            db.OrdersDetails.Add(oDetails);
            db.SaveChanges();
            // order.CustomerID = ovm.
            return View();
        }
        */
        // GET: admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            List<Customer> listCustomer = db.Customers.ToList();
            SelectList customerlist = new SelectList(listCustomer, "CustomerID", "Name");
            ViewBag.CustomerList = customerlist;
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Order order, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                string dropdownvalue = fc["CustomerID"].ToString();
                order.CustomerID = int.Parse(dropdownvalue);
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CreateOrderDetail()
        {
            List<Product> Proc = db.Products.ToList();
            SelectList ProcList = new SelectList(Proc, "ProductID", "ProductName");
            ViewBag.procList = ProcList;
            return View();
        }
        [HttpPost]
        public ActionResult CreateOrderDetail(OrderViewModel orderViewModel, FormCollection fc, int id)
        {

            if (ModelState.IsValid)
            {
                int product_id = int.Parse(fc["ProductID"].ToString());
                OrdersDetail odetails = new OrdersDetail();
                odetails.OrderID = id;
                odetails.Quantity = orderViewModel.Quantity;
                odetails.ProductID = product_id;
                odetails.Price = decimal.Parse(orderViewModel.Price) ;
               // odetails.Price =(decimal.Parse(orderViewModel.Price));
                db.OrdersDetails.Add(odetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
