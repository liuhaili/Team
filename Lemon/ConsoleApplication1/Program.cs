using Lemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lemon.Extensions;

namespace ConsoleApplication1
{
    public class Test
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Name4 { get; set; }
        public string Name5 { get; set; }
        public string Name6 { get; set; }
        public string Name7 { get; set; }
        public string Name8 { get; set; }
        public string Name9 { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Test t = new Test();
            string tips = t.Name9.GetName(it => t.Name9);
            Console.WriteLine(tips);

            string renren = "";
            string tips1 = renren.GetName(it => renren);
            Console.WriteLine(tips1);

            //WebApi("",2);

            //DateTime preTime = DateTime.Now;
            //for (int i = 0; i < 10000; i++)
            //{
            //    Test test = new Test();
            //    test.Name = "1";
            //    test.Name1 = "2";
            //    test.Name2 = "3";
            //    test.Name3 = "4";
            //    test.Name4 = "5";
            //    test.Name5 = "6";
            //    test.Name6 = "7";
            //    test.Name7 = "8";
            //    test.Name8 = "9";
            //    test.Name9 = "10";
            //    string sql = SqlConverter.ToInsertSQL(test);
            //}


            //Test test = new Test();
            //test.ID = 1;
            //test.Name = "aaa";
            //string sql = SqlConverter.ToUpdateSQL(test);

            //string sql = SqlConverter.ToDeleteSQL(typeof(Test), 2, "Name='aaa'");

            //string sql = SqlConverter.ToSelectSQL(typeof(Test), "Name='aaa'");
            //string sql = SqlConverter.ToSelectSQL(typeof(Test));

            //Console.Write("总耗时：" + (DateTime.Now - preTime).TotalMilliseconds);
            //Console.Write(sql);
            Console.Read();
        }

        public static void WebApi(string name, int age)
        {
            //for (int i = 0; i < pars.Length; i++)
            //{
            //    string pname = pars[i].GetName(c => pars[i]);
            //    Console.WriteLine(pname);

            //}
            //Console.WriteLine(name.GetName(c => name));
            //Console.WriteLine(age.GetName(c => age));
        }
    }
}
