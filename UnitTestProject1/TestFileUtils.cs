using Calc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class TestFileUtils
    {
        [TestMethod]
        public void Test1()
        {
            Assert.IsFalse(FileUtils.IsFileExists("d:/1.txt"));
            FileUtils.WriteFile("d:/1.txt", "abc你好");
            string s = FileUtils.ReadFile("d:/1.txt");
            Assert.AreEqual(s, "abc你好");
        }

        [ClassInitialize]
        public static void InitClass(TestContext ctx)
        {
            File.Delete("d:/1.txt");
        }
        /*
        [ClassCleanup]
        public static void CleanClass()
        {
            File.Delete("d:/1.txt");
        }*/
    }
}
