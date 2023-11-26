using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_new_lumiere.Models;

namespace web_new_lumiere.Controllers
{
    public class HomeController : Controller
    {
        private db_lumiere_newEntities db = new db_lumiere_newEntities();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                int? roleid = db.vw_user_roles.Where(u => u.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault().roleid;
                ViewBag.roleid = roleid;
            }


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Lumiere Shop by Aisha, Kika, Jesika.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Aisha, Kika, Jesika.";

            return View();
        }
    }
}