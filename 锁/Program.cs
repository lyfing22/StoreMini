using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 锁
{
    class Program
    {
        static void Main(string[] args)
        {
            string bf = Console.ReadLine();
            using (MyDbContext ctx = new MyDbContext())
            {
                ctx.Database.Log = (sql) =>
                {
                    Console.WriteLine(sql);
                };
                var g = ctx.Girls.First();
                if (!string.IsNullOrEmpty(g.BF))
                {
                    if (g.BF == bf)
                    {
                        Console.WriteLine("早已经是你的人了呀，还抢啥？");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("来晚了，早就被别人抢走了");
                        Console.ReadKey();
                        return;
                    }
                }
                Console.WriteLine("点击任意键，开抢（模拟耗时等待并发）");
                Console.ReadKey();
                g.BF = bf;
                try
                {
                    ctx.SaveChanges();
                    Console.WriteLine("抢媳妇成功");
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("抢媳妇失败");
                }
            }
            Console.ReadKey();

        }

        static void Main4(string[] args)
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                Console.WriteLine("请输入你的名字");
                string bf = Console.ReadLine();
                var g = ctx.Database.SqlQuery<Girl>("select * from T_Girls where id=1").Single();
                byte[] rowver = g.rowver;
                if (!string.IsNullOrEmpty(g.BF))
                {
                    if (g.BF == bf)
                    {
                        Console.WriteLine("早已经是你的人了呀，还抢啥？");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("来晚了，早就被别人抢走了");
                        Console.ReadKey();
                        return;
                    }
                }
                Console.WriteLine("点击任意键，开抢（模拟耗时等待并发）");
                Console.ReadKey();
                Thread.Sleep(3000);
                int affectRow = ctx.Database.ExecuteSqlCommand("update T_Girls set BF={0} where id=1 and RowVer={1}",
                    bf, rowver);
                if (affectRow == 0)
                {
                    Console.WriteLine("抢媳妇失败");
                }
                else if (affectRow == 1)
                {
                    Console.WriteLine("抢媳妇成功");
                }
                else
                {
                    Console.WriteLine("什么鬼");
                }
            }

            Console.ReadKey();

        }

        static void Main3(string[] args)
        {
            Console.WriteLine("请输入您的名字");
            string myname = Console.ReadLine();
            string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                long rowver = 0;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select id,name,bf,CONVERT(BIGINT,rowver) from T_Girls where Id=1";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            Console.WriteLine("没找到id=1的");
                            return;
                        }
                        string bf = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("BF")))
                        {
                            bf = reader.GetString(reader.GetOrdinal("BF"));
                        }
                        if (!string.IsNullOrEmpty(bf))
                        {
                            if (bf == myname)
                            {
                                Console.WriteLine("早就是我的人了");
                            }
                            else
                            {
                                Console.WriteLine(bf + "妻不客气");
                            }
                            Console.ReadKey();
                            return;
                        }
                        rowver = reader.GetInt64(reader.GetOrdinal("rowver"));
                    }
                }
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "update T_Girls set BF=@bf where Id=1 and rowver=@rv";
                    cmd.Parameters.Add(new SqlParameter("@bf", myname));
                    cmd.Parameters.Add(new SqlParameter("@rv", rowver));
                    int affectedRow = cmd.ExecuteNonQuery();
                    if (affectedRow <= 0)
                    {
                        Console.WriteLine("被别人抢先啦");
                    }
                    else
                    {
                        Console.WriteLine("被我抢到了");
                    }
                }
            }

            Console.ReadKey();
        }

        static void Main1(string[] args)
        {
            Console.WriteLine("请输入您的名字");
            string myname = Console.ReadLine();
            string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("开始查询");
                        using (var selectCmd = conn.CreateCommand())
                        {
                            selectCmd.Transaction = tx;
                            selectCmd.CommandText = "select * from T_Girls with(xlock,ROWLOCK) where id=1";
                            using (var reader = selectCmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    Console.WriteLine("没有id为1的女孩");
                                    return;
                                }
                                string bf = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("BF")))
                                {
                                    bf = reader.GetString(reader.GetOrdinal("BF"));
                                }
                                if (!string.IsNullOrEmpty(bf))//已经有男朋友
                                {
                                    if (bf == myname)
                                    {
                                        Console.WriteLine("早已经是我的人了");
                                    }
                                    else
                                    {
                                        Console.WriteLine("早已经被" + bf + "抢走了");
                                    }
                                    Console.ReadKey();
                                    return;
                                }
                                //如果bf==null，则继续向下抢
                            }
                            Console.WriteLine("查询完成，开始update");
                            using (var updateCmd = conn.CreateCommand())
                            {
                                updateCmd.Transaction = tx;
                                updateCmd.CommandText = "Update T_Girls set BF=@bf where id=1";
                                updateCmd.Parameters.Add(new SqlParameter("@bf", myname));
                                updateCmd.ExecuteNonQuery();
                            }
                            Console.WriteLine("结束Update");
                            Console.WriteLine("按任意键结束事务");
                            Console.ReadKey();
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        tx.Rollback();
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
