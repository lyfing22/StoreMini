using modelbinder1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace modelbinder1.Controllers
{
    public class DefaultController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /*
        [HttpPost]
        public ActionResult Index(User model)
        {
            //model.UserName = ToDBC(model.UserName.Trim());
           // model.Password = ToDBC(model.Password.Trim());

            if (model.UserName=="admin"&&model.Password=="123")
            {
                return Content("成功");
            }
            else
            {
                return Content("错误");
            }
        }*/

        /*
        [HttpPost]
        public ActionResult Index(string userName,string password)
        {
            //model.UserName = ToDBC(model.UserName.Trim());
            // model.Password = ToDBC(model.Password.Trim());

            if (userName == "admin" && password == "123")
            {
                return Content("成功");
            }
            else
            {
                return Content("错误");
            }
        }*/

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            //model.UserName = ToDBC(model.UserName.Trim());
            // model.Password = ToDBC(model.Password.Trim());
            string userName = fc["UserName"];
            string password = fc["Password"];
            if (userName == "admin" && password == "123")
            {
                return Content("成功");
            }
            else
            {
                return Content("错误");
            }
        }
    }
}