using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestIService;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class DefaultController : Controller
    {
        public IUserService UserService { get; set; }

        // GET: Default
        public ActionResult Index()
        {
            //bool b = UserService.CheckLogin("abc", "123");
            //TestHelper.Test();
            //return Content(b.ToString());
            //只有Autofac帮我们创建的对象才有可能给我们自动进行属性的赋值PropertiesAutowired
            Person p1 = DependencyResolver.Current.GetService<Person>();
            //Person p1 = new Person();
            p1.Hello();
            
            return Content("ok");
        }

        [HttpGet]
        public ActionResult TestJson()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestJson(FormCollection fc)
        {
            Dog dog = new Dog() { BirthDate=DateTime.Now,Id=5,Name="旺财"};
            return Json(dog);
            //return new JsonNetResult() { Data=dog};
        }

        [HttpGet]
        public ActionResult Test2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Test2(Test2Model model)
        //public ActionResult Test2(string name,int age)
        //public ActionResult Test2(FormCollection fc)
        {
            //return Content(name + age);
            return Content(model.Name+model.Age);
            /*
            string name = fc["name"];
            string age = fc["age"];
            return Content(name + age);*/
        }

        public ActionResult Pager1()
        {
            return View();
        }
    }
}