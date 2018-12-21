using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace memcachedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            config.Servers.Add(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11211));
            config.Protocol = MemcachedProtocol.Binary;
            MemcachedClient client = new MemcachedClient(config);
            var p = new Person { Id = 3, Name = "yzk" };
            //保存到缓存中
            //HttpContext.Cache.Insert(cacheKey, model, null,
            //DateTime.Now.AddSeconds(1),TimeSpan.Zero);
            client.Store(StoreMode.Set, "p"+p.Id, p,DateTime.Now.AddSeconds(3));//还可以指定第四个参数指定数据的过期时间。
            Thread.Sleep(2000);
            
            Person p1 = client.Get<Person>("p3");//HttpContext.Cache[cacheKey]
            Console.WriteLine(p1.Name);
            Thread.Sleep(2000);
            p1 = client.Get<Person>("p3");
            Console.WriteLine(p1.Name);
            Console.ReadKey();
        }
    }
}
