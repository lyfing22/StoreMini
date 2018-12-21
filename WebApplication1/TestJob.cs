using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebApplication1
{
    public class TestJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
           // if (....) { return; }
            try
            {
                string path = HostingEnvironment.MapPath("~/Web.config");
            //string path = HttpContext.Current.Server.MapPath("~/Web.config");
            File.AppendAllText(@"d:/log.txt", path+"\r\n");
                //File.AppendAllText(@"d:/log.txt", "执行啦" + DateTime.Now);
            }
            catch(Exception ex)
            {
                File.AppendAllText(@"d:/log.txt", "出错啦"+ex + "\r\n");
            }

        }
    }
}