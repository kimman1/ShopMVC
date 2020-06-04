using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop;
using Shop.Models;

namespace Shop.Areas.admin.Controllers
{
    public class CategoriesController : Controller
    {
        private ShopMVCEntities db = new ShopMVCEntities();

        // GET: admin/Categories
        public ActionResult Index()
        {
            var p = db.Categories.Select(s => s);
            //return View(db.Categories.ToList());
            return View(p);
        }

        // GET: admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            ObjectParameter returnValue = new ObjectParameter("outputresult", typeof(int));
            db.deleteCategory(id, returnValue);
            int result = Convert.ToInt32(returnValue.Value);
            //db.Categories.Remove(category);
            if (result == 0)
            {
                ViewBag.status = "Xóa thất bại. Kiểm tra Products!!!";
                return View(category);
            }
            else
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           
        }
       
        public ActionResult RedirectToCustomer()
        {
            return RedirectToAction("Index","Customers");
        }
        public ActionResult RedirectToOrder()
        {
            return RedirectToAction("Index", "Orders");
        }
        public ActionResult RedirectToProduct()
        {
            return RedirectToAction("Index", "Products");
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
