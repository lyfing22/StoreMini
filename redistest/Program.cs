using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace redistest
{
    class Program
    {
        static void Main(string[] args)
        {
            PooledRedisClientManager redisMgr = new PooledRedisClientManager("127.0.0.1");
            using (IRedisClient redisClient = redisMgr.GetClient())
            {
                var p = new Person { Id = 3, Name = "yzk" };
                //redisClient.Set("p", p,DateTime.Now.AddSeconds(3));
                redisClient.Set("p3", p, TimeSpan.FromSeconds(3));
                Thread.Sleep(2000);
                var p1 = redisClient.Get<Person>("p3");
                Console.WriteLine(p1.Name);
                Thread.Sleep(2000);
                p1 = redisClient.Get<Person>("p3");
                Console.WriteLine(p1.Name);
            }

        }
    }
}
