﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service;

namespace ServiceTests
{
    [TestClass]
    public class AdminUserServiceTest
    {
        private AdminUserService userService = new AdminUserService();
        [TestMethod]
        public void TestAddAdminUser()
        {
            long uid =
                userService.AddAdminUser("abc", "189181", "123", "123@qq.com", null);
            var user = userService.GetById(uid);
            Assert.AreEqual(user.Name, "abc");
            Assert.AreEqual(user.PhoneNum, "189181");
            Assert.AreEqual(user.Email, "123@qq.com");
            Assert.IsNull(user.CityId);
            Assert.IsTrue(userService.CheckLogin("189181", "123"));
            Assert.IsFalse(userService.CheckLogin("189181", "abc"));
            userService.GetAll();
            Assert.IsNotNull(userService.GetByPhoneNum("189181"));
            userService.MarkDeleted(uid);//为了保证TestCase可以重复执行，那么把创建的数据干掉
        }

        [TestMethod]
        public void TestHasPerm()
        {
            try
            {
                PermissionService permService = new PermissionService();
                string permName1 = Guid.NewGuid().ToString();
                long permId1 = permService.AddPermission(permName1, permName1);
                string permName2 = Guid.NewGuid().ToString();
                long permId2 = permService.AddPermission(permName2, permName2);

                RoleService roleService = new RoleService();
                string roleName1 = Guid.NewGuid().ToString();
                long roleId1 = roleService.AddNew(roleName1);

                string userPhone = "139139";
                long userId = userService.AddAdminUser("aaa", userPhone, "123", "123@qq.com", null);

                roleService.AddRoleIds(userId, new long[] { roleId1 });
                permService.AddPermIds(roleId1, new long[] { permId1 });
                Assert.IsTrue(userService.HasPermission(userId, permName1));
                Assert.IsFalse(userService.HasPermission(userId, permName2));

                userService.MarkDeleted(userId);
            }
            catch (DbEntityValidationException ex)
            {
                foreach(var err in ex.EntityValidationErrors.SelectMany(err=>err.ValidationErrors))
                {
                    Console.WriteLine(err.ErrorMessage);
                }
                throw;
            }
        }

    }
}
