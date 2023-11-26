using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_new_lumiere.Models;

namespace web_new_lumiere.Controllers
{
    public class ProductController : Controller
    {
        private db_lumiere_newEntities db = new db_lumiere_newEntities();

        // GET: Product
        [Authorize]
        public ActionResult Index(int pageno=1)
        {
            //return View(db.vw_product.ToList());
            pageno = pageno - 1;
            List<vw_product> products = db.vw_product.
                                        OrderBy(p => p.pid).Skip(12 * pageno).Take(12).ToList();


            int? roleid = db.vw_user_roles.Where(u=>u.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault().roleid;

            ViewBag.username = User.Identity.Name.ToString();
            ViewBag.pageno = pageno;
            ViewBag.roleid = roleid;

            return View(products);
        }

        [Authorize]
        public ActionResult Search()
        {
            return View();
        }

        [Authorize]
        public ActionResult Result(int pageno, String category_code, String sub_category_code, float minprice, float maxprice)
        {
            pageno = pageno-1;
            List<vw_product> products = db.vw_product.
                                        Where(p=>p.category_code.Equals(category_code.Trim())).
                                        Where(p=>p.subcategory_code.Equals(sub_category_code.Trim())).
                                        Where(p=>p.product_price>=minprice && p.product_price<=maxprice).
                                        OrderBy(p=>p.pid).Skip(12 * pageno).Take(12).ToList();


            ViewBag.pageno = pageno;
            ViewBag.category_code = category_code;
            ViewBag.sub_category_code = sub_category_code;
            ViewBag.minprice = minprice;
            ViewBag.maxprice = maxprice;

            int? roleid = db.vw_user_roles.Where(u => u.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault().roleid;
            ViewBag.roleid = roleid;

            return View(products);
        }

        // GET: Product/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_product vw_product = db.vw_product.Find(id);
            if (vw_product == null)
            {
                return HttpNotFound();
            }
            return View(vw_product);
        }

        // GET: Product/Create
        [Authorize]
        public ActionResult Create()
        {

            int? roleid = db.vw_user_roles.Where(u => u.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault().roleid;

            if (roleid > 0)
            {
                return RedirectToAction("Index");
            }

            var categoryitems = new[]
            {
                new SelectListItem { Value = "dry", Text = "Dry" },
                new SelectListItem { Value = "oily", Text = "Oily" },
                new SelectListItem { Value = "combination", Text = "Combination" }
            };

            ViewBag.category = categoryitems;

            var subcategoryitems = new[]
            {
                new SelectListItem { Value = "cleanser", Text = "Cleanser" },
                new SelectListItem { Value = "face mask", Text = "Face mask" },
                new SelectListItem { Value = "mosturizer", Text = "Mosturizer" },
                new SelectListItem { Value = "serum", Text = "Serum" },
                new SelectListItem { Value = "sunscreen", Text = "Sunscreen" },
                new SelectListItem { Value = "toner", Text = "Toner" }
            };

            ViewBag.subcategory = subcategoryitems;

            var branditems = new[]
            {
                new SelectListItem { Value = "azarine", Text = "Azarine" },
                new SelectListItem { Value = "carasun", Text = "Carasun" },
                new SelectListItem { Value = "elformula", Text = "Elformula" },
                new SelectListItem { Value = "joylab", Text = "Joylab" },
                new SelectListItem { Value = "safi", Text = "Safi" },
                new SelectListItem { Value = "skintific", Text = "Skintific" },
                new SelectListItem { Value = "somethinc", Text = "Somethinc" },
                new SelectListItem { Value = "wardah", Text = "Wardah" }
            };

            ViewBag.brand = branditems;

            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "pid,product_id,pid_category,category_code,pid_subcategory,subcategory_code,pid_brand,brand_code,product_desc,product_url,product_size,product_price")] vw_product vw_product)
        {
            if (ModelState.IsValid)
            {

                int? roleid = db.vw_user_roles.Where(u => u.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault().roleid;
                if (roleid > 0)
                {
                    return RedirectToAction("Index");
                }

                tbl_product product = new tbl_product();
                product.pid = Guid.NewGuid().ToString();
                product.category_code = vw_product.pid_category;
                product.subcategory_code = vw_product.pid_subcategory;
                product.brand_code = vw_product.pid_brand;
                product.product_size = vw_product.product_size;
                product.product_price = vw_product.product_price;
                product.product_desc = vw_product.product_desc;
                product.product_id = vw_product.product_id;
                product.product_name = vw_product.product_name;
                product.product_url = vw_product.product_url;
                db.tbl_product.Add(product);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(vw_product);
        }

        // GET: Product/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

 
            var categoryitems = new[]
            {
                new SelectListItem { Value = "dry", Text = "Dry" },
                new SelectListItem { Value = "oily", Text = "Oily" },
                new SelectListItem { Value = "combination", Text = "Combination" }
            };

            ViewBag.category = categoryitems;

            var subcategoryitems = new[]
            {
                new SelectListItem { Value = "cleanser", Text = "Cleanser" },
                new SelectListItem { Value = "face mask", Text = "Face mask" },
                new SelectListItem { Value = "mosturizer", Text = "Mosturizer" },
                new SelectListItem { Value = "serum", Text = "Serum" },
                new SelectListItem { Value = "sunscreen", Text = "Sunscreen" },
                new SelectListItem { Value = "toner", Text = "Toner" }
            };

            ViewBag.subcategory = subcategoryitems;

            var branditems = new[]
            {
                new SelectListItem { Value = "azarine", Text = "Azarine" },
                new SelectListItem { Value = "carasun", Text = "Carasun" },
                new SelectListItem { Value = "elformula", Text = "Elformula" },
                new SelectListItem { Value = "joylab", Text = "Joylab" },
                new SelectListItem { Value = "safi", Text = "Safi" },
                new SelectListItem { Value = "skintific", Text = "Skintific" },
                new SelectListItem { Value = "somethinc", Text = "Somethinc" },
                new SelectListItem { Value = "wardah", Text = "Wardah" }
            };

            ViewBag.brand = branditems;

            vw_product vw_product = db.vw_product.Find(id);
            if (vw_product == null)
            {
                return HttpNotFound();
            }
            return View(vw_product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "pid,product_id,pid_category,category_code,pid_subcategory,subcategory_code,pid_brand,brand_code,product_name,product_desc,product_url,product_size,product_price")] vw_product vw_product)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(vw_product).State = EntityState.Modified;

                tbl_product product = db.tbl_product.Find(vw_product.pid);
                product.category_code = vw_product.pid_category;
                product.subcategory_code = vw_product.pid_subcategory;
                product.brand_code = vw_product.pid_brand;
                product.product_size = vw_product.product_size;
                product.product_price = vw_product.product_price;   
                product.product_desc = vw_product.product_desc;
                product.product_id = vw_product.product_id;
                product.product_name = vw_product.product_name;
                product.product_url = vw_product.product_url;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vw_product);
        }

        // GET: Product/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_product item = db.tbl_product.Find(id);
            //vw_product vw_product = db.vw_product.Find(id);

            if (item == null)
            {
                return HttpNotFound();
            }

            db.tbl_product.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            vw_product vw_product = db.vw_product.Find(id);
            db.vw_product.Remove(vw_product);
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
