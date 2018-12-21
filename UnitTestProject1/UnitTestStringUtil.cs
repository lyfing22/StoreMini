using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calc;

namespace UnitTestProject1
{
    /// <summary>
    /// UnitTestStringUtil 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTestStringUtil
    {

        [TestMethod]
        public void TestIsEmail()
        {
            //Console.WriteLine("hello");
            Assert.IsTrue(StringUtils.IsEmail("abc@rupeng.com"));
            Assert.IsFalse(StringUtils.IsEmail("rupeng.com"));
            if(StringUtils.IsEmail("abc@rupeng.com")!=true)
            {
                Assert.Fail();
            }
           // CollectionAssert.
        }
    }
}
