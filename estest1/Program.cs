using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace estest1
{
    class Program
    {
        static void Main(string[] args)
        {
            ElasticConnection client = new ElasticConnection("localhost", 9200);
            SearchCommand cmd = new SearchCommand("movies", "电影");
            var query = new QueryBuilder<VerycdItem>()
            .Query(b =>
                    b.Bool(m =>
                    //并且关系
                    m.Must(t =>
                       //分词的最小单位或关系查询
                       t.QueryString(t1 => t1.DefaultField("content").Query("成龙"))
                         ).Must(t=>t.QueryString(t1=>t1.DefaultField("category2").Query("纪录")))
                      )
                    ).Size(100)
            .Build();
           // DeleteCommand delCmd = new DeleteCommand()
            var result = client.Post(cmd, query);
            var serializer = new JsonNetSerializer();
            var searchResult = serializer.ToSearchResult<VerycdItem>(result);
            //searchResult.hits.total; //一共有多少匹配结果  10500
            // searchResult.Documents;//当前页的查询结果 
            foreach (var doc in searchResult.Documents)
            {
                Console.WriteLine(doc.title+","+doc.category1 + "," + doc.category2);
            }
            Console.ReadKey();
        }

        static void Main4(string[] args)
        {

            using (SQLiteConnection conn
                = new SQLiteConnection(@"Data Source=G:\其他资料\技术资料\各种海量数据\VeryCD\verycd.sqlite3.db"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from verycd";
                    using (var reader = cmd.ExecuteReader())
                    {
                        ElasticConnection client = new ElasticConnection("localhost", 9200);
                        var serializer = new JsonNetSerializer();
                        while (reader.Read())
                        {
                            long verycdid = reader.GetInt64(reader.GetOrdinal("verycdid"));
                            string title = reader.GetString(reader.GetOrdinal("title"));
                            string status = reader.GetString(reader.GetOrdinal("status"));
                            string brief = reader.GetString(reader.GetOrdinal("brief"));
                            string pubtime = reader.GetString(reader.GetOrdinal("pubtime"));
                            string updtime = reader.GetString(reader.GetOrdinal("updtime"));
                            string category1 = reader.GetString(reader.GetOrdinal("category1"));
                            string category2 = reader.GetString(reader.GetOrdinal("category2"));
                            string ed2k = reader.GetString(reader.GetOrdinal("ed2k"));
                            string content = reader.GetString(reader.GetOrdinal("content"));
                            string related = reader.GetString(reader.GetOrdinal("related"));

                            VerycdItem item = new VerycdItem();
                            item.verycdid = verycdid;
                            item.title = title;
                            item.status = status;
                            item.brief = brief;
                            item.pubtime = pubtime;
                            item.updtime = updtime;
                            item.category1 = category1;
                            item.category2 = category2;
                            item.ed2k = ed2k;
                            item.content = content;
                            item.related = related;

                            Console.WriteLine("当前读取到id=" + verycdid);
                            IndexCommand indexCmd = new IndexCommand("verycd", "items", verycdid.ToString());
                            //Put()第二个参数是要插入的数据
                            OperationResult result = client.Put(indexCmd, serializer.Serialize(item));
                            var indexResult = serializer.ToIndexResult(result.Result);
                            if (indexResult.created)
                            {
                                Console.WriteLine("创建了");
                            }
                            else
                            {
                                Console.WriteLine("没创建" + indexResult.error);
                            }
                        }
                    }
                }
            }



            Console.ReadKey();
        }

        static void Main2(string[] args)
        {
            ElasticConnection client = new ElasticConnection("localhost", 9200);
            SearchCommand cmd = new SearchCommand("zsz", "persons");
            var query = new QueryBuilder<Person>()
            .Query(b =>
                    b.Bool(m =>
                    //并且关系
                    m.Must(t =>
                       //分词的最小单位或关系查询
                       t.QueryString(t1 => t1.DefaultField("Name").Query("帅"))
                         )
                      )
                    )
            //分页
            /*
            .From(0)//Skip()
            .Size(10)//Take()*/
            //排序
            //.Sort(c => c.Field("Age", SortDirection.desc))
            //添加高亮
            /*
            .Highlight(h => h
            .PreTags("<b>")
            .PostTags("</b>")
            .Fields(
                 f => f.FieldName("Name").Order(HighlightOrder.score)
                 )
            )*/
            .Build();
            var result = client.Post(cmd, query);
            var serializer = new JsonNetSerializer();
            var searchResult = serializer.ToSearchResult<Person>(result);
            //searchResult.hits.total; //一共有多少匹配结果  10500
            // searchResult.Documents;//当前页的查询结果 
            foreach (var doc in searchResult.Documents)
            {
                Console.WriteLine(doc.Id + "," + doc.Name + "," + doc.Age);
            }
            Console.ReadKey();
        }

        static void Main1(string[] args)
        {
            /*
            Person p1 = new Person();
            p1.Id = 1;
            p1.Age = 10;
            p1.Name = "欧阳帅帅";
            p1.Desc = "欧阳锋家的帅哥公子，人送外号‘小杨中科’";*/

            Person p1 = new Person();
            p1.Id = 2;
            p1.Age = 8;
            p1.Name = "丑娘娘";
            p1.Desc = "二丑家的姑娘，是最美丽的女孩";

            ElasticConnection client = new ElasticConnection("localhost", 9200);
            var serializer = new JsonNetSerializer();
            //第一个参数相当于“数据库”，第二个参数相当于“表”，第三个参数相当于“主键”
            IndexCommand cmd = new IndexCommand("zsz", "persons", p1.Id.ToString());
            //Put()第二个参数是要插入的数据
            OperationResult result = client.Put(cmd, serializer.Serialize(p1));
            var indexResult = serializer.ToIndexResult(result.Result);
            if (indexResult.created)
            {
                Console.WriteLine("创建了");
            }
            else
            {
                Console.WriteLine("没创建" + indexResult.error);
            }
            Console.ReadKey();
        }
    }
}
