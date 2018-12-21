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
    public class RBACUnitTest
    {
        private static AdminUserService userService = new AdminUserService();
        private static PermissionService permService = new PermissionService();
        private static RoleService roleService = new RoleService();

        private static long userId;
        [TestMethod]
        public void TestUserRole()
        {            
            string permName1 = Guid.NewGuid().ToString();
            long permId1 = permService.AddPermission(permName1, permName1);
            string permName2 = Guid.NewGuid().ToString();
            long permId2 = permService.AddPermission(permName2, permName2);

            
            string roleName1 = Guid.NewGuid().ToString();
            long roleId1 = roleService.AddNew(roleName1);

            string roleName2 = Guid.NewGuid().ToString();
            long roleId2 = roleService.AddNew(roleName2);

            string userPhone = "178158";
            userId = userService.AddAdminUser("aaa", userPhone, "123", "123@qq.com", null);

            roleService.AddRoleIds(userId, new long[] { roleId1 });
            permService.AddPermIds(roleId1, new long[] { permId1 });
            Assert.IsTrue(userService.HasPermission(userId, permName1));
            //Assert.IsFalse(userService.HasPermission(userId, permName2));

            roleService.UpdateRoleIds(userId, new long[] { roleId2 });
            Assert.IsFalse(userService.HasPermission(userId, permName1));
            CollectionAssert.AreEqual(roleService.GetByAdminUserId(userId).Select(r => r.Id).ToArray(), 
                new long[] { roleId2 });

            
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            if(userId!=0)
            {
                userService.MarkDeleted(userId);
            }            
        }
    }
}
