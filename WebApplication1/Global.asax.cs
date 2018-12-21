using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApplication1
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IScheduler sched = new StdSchedulerFactory().GetScheduler();
            JobDetailImpl jdBossReport = new JobDetailImpl("jdTest", typeof(TestJob));

            CalendarIntervalScheduleBuilder builder = 
                CalendarIntervalScheduleBuilder.Create();
            builder.WithInterval(5, IntervalUnit.Second);
            

            IMutableTrigger triggerBossReport = builder.Build();
            triggerBossReport.Key = new TriggerKey("triggerTest");
            sched.ScheduleJob(jdBossReport, triggerBossReport);
            sched.Start();

        }
    }
}