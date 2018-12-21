using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service;

namespace ServiceTests
{
    [TestClass]
    public class IdNameUnitTest
    {
        private IdNameService inSvc = new IdNameService(); 
        [TestMethod]
        public void TestIdName()
        {
            string typeName = Guid.NewGuid().ToString();
            string name1 = Guid.NewGuid().ToString();
            string name2 = Guid.NewGuid().ToString();
            long id1 = inSvc.AddNew(typeName,name1);
            long id2 = inSvc.AddNew(typeName, name2);
            Assert.AreEqual(inSvc.GetById(id1).Name, name1);
            Assert.AreEqual(inSvc.GetAll(typeName).Length, 2);
            Assert.AreEqual(inSvc.GetAll(typeName)[1].Name,name2);
        }
    }
}
