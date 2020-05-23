using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop;
using Shop.Areas.admin.ViewModel;

namespace Shop.Areas.admin.Controllers
{
    public class ProductsController : Controller
    {
        private ShopMVCEntities db = new ShopMVCEntities();

        // GET: admin/Products
        public ActionResult Index()
        {
            var p = db.Products.Select(s => s);
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            foreach (Product product in p)
            {
                ProductViewModel pvm = new ProductViewModel();
                pvm.CategoryID = (int)product.CategoryID;
                pvm.InStock = (int)product.InStock;
                pvm.ProductID = product.ProductID;
                pvm.ProductName = product.ProductName;
                pvm.UnitPrice = (int)product.UnitPrice;
                pvm.category = db.Categories.Where(s => s.CategoryID == product.CategoryID).FirstOrDefault();
                productViewModels.Add(pvm);
            }
            return View(productViewModels);
        }

        // GET: admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            ProductViewModel pvm = new ProductViewModel();
            pvm.CategoryID = (int) product.CategoryID;
            pvm.category = db.Categories.Where(s => s.CategoryID == product.CategoryID).FirstOrDefault();
            pvm.InStock = (int) product.InStock;
            pvm.ProductID = product.ProductID;
            pvm.ProductName = product.ProductName;
            pvm.UnitPrice = (int) product.UnitPrice;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(pvm);
        }

        // GET: admin/Products/Create
        public ActionResult Create()
        {
            //ViewBag.ProductID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            List<Category> listcate = db.Categories.ToList();
            SelectList catelist = new SelectList(listcate, "CategoryID", "CategoryName");
            ViewBag.CatList = catelist;
            return View();
        }

        // POST: admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, FormCollection fc)
        {
            int catID = int.Parse(fc["CateNameDrop"].ToString());
            if (ModelState.IsValid)
            {
                //product.CategoryID = catID;
                Product p = new Product();
                p.ProductName = product.ProductName;
                p.UnitPrice = product.UnitPrice;
                p.InStock = product.InStock;
                p.CategoryID = catID;
                db.Products.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            //ViewBag.ProductID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.ProductID);
            return View(product);
        }

        // GET: admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            ProductViewModel pvm = new ProductViewModel();
            pvm.CategoryID = (int)product.CategoryID;
            pvm.category = db.Categories.Where(s => s.CategoryID == product.CategoryID).FirstOrDefault();
            pvm.InStock = (int)product.InStock;
            pvm.ProductID = product.ProductID;
            pvm.ProductName = product.ProductName;
            pvm.UnitPrice = (int)product.UnitPrice;
            List<Category> listcate = db.Categories.ToList();
            SelectList catelist = new SelectList(listcate, "CategoryID", "CategoryName");
            ViewBag.CateList = catelist;
            if (product == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ProductID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.ProductID);
            return View(pvm);
        }

        // POST: admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                string dropdrowID = fc["DropDownCate"].ToString();
                product.CategoryID = int.Parse(dropdrowID);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
