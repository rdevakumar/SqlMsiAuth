using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSqlAuth_SystemDataSqlClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            // ViewBag.Message = "Your contact page.";
            // ViewBag.Message = Utils.SqlHelper.SqlAuthVmIdentity();
            ViewBag.Message = Utils.SqlHelper.SqlAuthVmIdentityNew();
            return View();
        }
    }
}