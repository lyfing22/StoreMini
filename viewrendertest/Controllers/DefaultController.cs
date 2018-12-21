using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace viewrendertest.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            string html = RenderViewToString(this.ControllerContext,"~/Views/Default/Test.cshtml",
                "hello");
            System.IO.File.WriteAllText("d:/1.txt", html);
            return Content("ok");
        }

        static string RenderViewToString(ControllerContext context,
                string viewPath,
                object model = null)
        {
            ViewEngineResult viewEngineResult =
            ViewEngines.Engines.FindView(context, viewPath, null);
            if (viewEngineResult == null)
                throw new FileNotFoundException("View" + viewPath + "cannot be found.");
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                context.Controller.ViewData,
                                context.Controller.TempData,
                                sw);
                view.Render(ctx, sw);
                return sw.ToString();
            }
        }

    }
}