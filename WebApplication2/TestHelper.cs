using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestIService;

namespace WebApplication2
{
    public class TestHelper
    {
        public static void Test()
        {
            IUserService svc = DependencyResolver.Current.GetService<IUserService>();
            bool b = svc.CheckLogin("", "");
        }
    }
}