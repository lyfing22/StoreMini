using Autofac;
using Autofac.Integration.Mvc;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestIService;

namespace WebApplication2
{
    public class TestJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                /*
            IUserService svc = 
                DependencyResolver.Current.GetService<IUserService>();
    */
                IUserService svc;
                var container = AutofacDependencyResolver.Current.ApplicationContainer;
                using (container.BeginLifetimeScope())
                {
                    svc = container.Resolve<IUserService>();
                }

                bool b = svc.CheckLogin("", "");
            }
            catch(Exception ex)
            {

            }

        }
    }
}