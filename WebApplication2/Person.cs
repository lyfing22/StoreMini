using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestIService;

namespace WebApplication2
{
    public class Person
    {
        public IUserService userSvc { get; set; }

        public void Hello()
        {
            userSvc.CheckLogin("", "");
        }
    }
}