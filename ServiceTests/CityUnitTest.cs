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
    public class CityUnitTest
    {
        [TestMethod]
        public void Test1()
        {
            string cityName = DateTime.Now.ToFileTimeUtc().ToString();
            CityService citySvc = new CityService();
            long cityId = citySvc.AddNew(cityName);
            Assert.AreEqual(citySvc.GetById(cityId).Name, cityName);
            citySvc.GetAll();
            
        }

        [TestMethod]
        public void Test2()
        {
            RegionService regionSvc = new RegionService();
            var region = regionSvc.GetById(1);
            Assert.AreEqual(region.CityName, "北京");
            Assert.AreEqual(region.Name, "海淀区");
            Assert.AreEqual(regionSvc.GetAll(1).Length, 2);

            CommunityService comSvc = new CommunityService();
            Assert.AreEqual(comSvc.GetByRegionId(1).Length, 2);
        }
    }
}
