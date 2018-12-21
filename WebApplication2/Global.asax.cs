using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using System.Reflection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using ZSZ.CommonMVC;

namespace WebApplication2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new JsonNetActionFilter());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            //using Autofac.Integration.Mvc;
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .PropertiesAutowired();//把当前程序集中的Controller都注册

            Assembly asmService = Assembly.Load("TestService");
            builder.RegisterAssemblyTypes(asmService).Where(t=>!t.IsAbstract)
                .AsImplementedInterfaces().PropertiesAutowired();
            /*
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .PropertiesAutowired();*/

            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            /*
            IScheduler sched = new StdSchedulerFactory().GetScheduler();
            JobDetailImpl jdBossReport = new JobDetailImpl("jdTest", typeof(TestJob));
            CalendarIntervalScheduleBuilder scbuilder = CalendarIntervalScheduleBuilder.Create();
            
                scbuilder.WithInterval(5, IntervalUnit.Second);
            IMutableTrigger triggerBossReport = scbuilder.Build();

            triggerBossReport.Key = new TriggerKey("triggerTest");
            sched.ScheduleJob(jdBossReport, triggerBossReport);
            sched.Start();
            */
            ModelBinders.Binders.Add(typeof(string), 
                new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(int),
                new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(long),
                new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(double),
                new TrimToDBCModelBinder());
        }
    }
}
