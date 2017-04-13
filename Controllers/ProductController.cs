using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TryMvcApp.Models;

namespace TryMvcApp.Controllers
{
    public class ProductController : MainController
    {
        public ActionResult Index()
        {
            return RedirectToAction("List", "Product");
        }
        
        public ActionResult List(string productCategory, string searchString)
        {
            var categList = new List<string>();

            var categQuery = from d in AppDb.Products
                             select d.Category;

            categList.AddRange(categQuery.Distinct());
            ViewBag.productCategory = new SelectList(categList);

            var products = from m in AppDb.Products
                           select m;
            
            if (!string.IsNullOrEmpty(productCategory))
            {
                products = products.Where(x => x.Category == productCategory);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }

            return View(products);
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("List", "Product");
            }

            var product = AppDb.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Category,Price,OldPrice,Description")] ProductModel product)
        {
            if (!ModelState.IsValid) return View(product);

            AppDb.Products.Add(product);
            AppDb.SaveChanges();

            return RedirectToAction("List", "Product");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("List", "Product");
            }

            var product = AppDb.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Category,Price,OldPrice,Description")] ProductModel product)
        {
            if (!ModelState.IsValid) return View(product);

            AppDb.Entry(product).State = System.Data.Entity.EntityState.Modified;
            AppDb.SaveChanges();

            return RedirectToAction("List", "Product");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("List", "Product");
            }

            var product = AppDb.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = AppDb.Products.Find(id);
            AppDb.Products.Remove(product);
            AppDb.SaveChanges();

            return RedirectToAction("List", "Product");
        }
    }
}