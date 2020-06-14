using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebSockets;
using Shop;
using Shop.Areas.admin.ViewModel;

namespace Shop.Areas.admin.Controllers
{
    public class OrdersController : Controller
    {
        private ShopMVCEntities db = new ShopMVCEntities();

        // GET: admin/Orders
        // O***************************************** ORDER *********************************/
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        public ActionResult Details(int? id)
        {
            List<OrdersDetail> p = db.OrdersDetails.Where(m => m.OrderID == id).ToList();
            List<OrderViewModel> viewmodel = new List<OrderViewModel>();
            ViewBag.orderid = id;

            foreach (OrdersDetail odd in p)
            {
                OrderViewModel od = new OrderViewModel();
                //od.OrderDetailsID = odd.OrdersDetailID;
                od.OrderID = odd.OrderID;
                od.Price = odd.Price.ToString(); ;
                od.Product = odd.Product;
                od.Quantity = (int)odd.Quantity;
                viewmodel.Add(od);
            }
            return View(viewmodel);
        }

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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order, FormCollection fc)
        {
                
                    if (ModelState.IsValid)
                    {

                        string dropdownID = fc["DropDownCustomer"].ToString();
                        order.CustomerID = int.Parse(dropdownID);
                        order.TongTien = 0 + "";
                        order.OrderDate = DateTime.Now;
                        db.Orders.Add(order);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
            
            return View(order);
        }
      
     
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
            ObjectParameter returnValue = new ObjectParameter("outputresult", typeof(int));
            db.deleteOrders(id, returnValue);
            int result = Convert.ToInt32(returnValue.Value);
            if (result == 0)
            {
                ViewBag.status = "Xóa thất bại. Vui lòng kiểm tra Orders Detail!!!";
                return View(order);
            }
            else
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            
        }
        public ActionResult CreateOrderDetail()
        {
            List<Product> Proc = db.Products.ToList();
            SelectList ProcList = new SelectList(Proc, "ProductID", "ProductName");
            ViewBag.procList = ProcList;
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrderDetail(OrderViewModel orderViewModel, FormCollection fc, int id)
        {
            List<Product> Proc = db.Products.ToList();
            SelectList ProcList = new SelectList(Proc, "ProductID", "ProductName");
            ViewBag.procList = ProcList;
            if (ModelState.IsValid)
            {
                bool flag = false;
                int product_id = int.Parse(fc["ProductID"].ToString());
                Order od = db.Orders.Where(s => s.OrderID == id).FirstOrDefault();
                Product pd = db.Products.Where(s => s.ProductID == product_id).FirstOrDefault();
                List<int> listIdProduct= db.OrdersDetails.Where(s => s.OrderID == id).Select(s => s.ProductID).ToList();
                foreach (int idproduct in listIdProduct)
                {
                    if (idproduct == product_id)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    TempData["status"]  = "Sản phẩm bạn thêm đã có trong giỏ hàng";
                    TempData["orderID"] = id;
                    return RedirectToAction("Problem" ,"Problem");
                   
                    
                }
                else
                {
                    OrdersDetail odetails = new OrdersDetail();
                    odetails.OrderID = id;
                    odetails.Quantity = orderViewModel.Quantity;
                    odetails.ProductID = product_id;
                    odetails.Price = pd.UnitPrice*orderViewModel.Quantity;
                    db.OrdersDetails.Add(odetails);
                    od.TongTien = (decimal.Parse(od.TongTien) + orderViewModel.Quantity * pd.UnitPrice).ToString();
                    db.SaveChanges();
                    return RedirectToAction("Details", new {id = id });
                }

            }
            else
            {
                return View(orderViewModel);
                //return RedirectToAction("CreateOrderDetail", id);
            }
        }
        public ActionResult RedirectToCategories()
        {
            return RedirectToAction("Index","Categories");
        }
        public ActionResult RedirectToCustomer()
        {
            return RedirectToAction("Index", "Customers");
        }
        public ActionResult RedirectToProducts()
        {
            return RedirectToAction("Index", "Products");
        }
        public ActionResult DeleteOrderDetail(int? Productid, int? orderid)
        {

             OrdersDetail p = db.OrdersDetails.Where(s => s.ProductID == Productid && s.OrderID == orderid).FirstOrDefault();
             OrderViewModel odm = new OrderViewModel();
             odm.OrderID = p.OrderID;
             odm.Price =  p.Price.ToString();
             odm.Quantity = (int) p.Quantity;
             odm.Product = p.Product;
             return View(odm);
             
        }
        [HttpPost, ActionName("DeleteOrderDetail")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrderDetailConfirmed(int Productid, int orderid)
        {
             Order od = db.Orders.Where(s => s.OrderID == orderid).FirstOrDefault();
             OrdersDetail p = db.OrdersDetails.Where(s => s.ProductID == Productid && s.OrderID == orderid).FirstOrDefault();
             Product pd = db.Products.Where(s => s.ProductID == p.ProductID).FirstOrDefault();
             int orderId = p.OrderID;
             db.OrdersDetails.Remove(p);
             db.SaveChanges();
             od.TongTien = (decimal.Parse( (od.TongTien)) - (pd.UnitPrice * p.Quantity)).ToString();
             db.Entry(od).State = EntityState.Modified;
             db.SaveChanges();
             return RedirectToAction("Details",new {id = orderId});
            
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
