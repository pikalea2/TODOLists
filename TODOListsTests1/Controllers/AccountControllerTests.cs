using Microsoft.VisualStudio.TestTools.UnitTesting;
using TODOLists.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOLists.Models;
using System.Web.Mvc;

namespace TODOLists.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        [TestMethod()]
        public void IsValidUserTest()
        {
            AccountController accountcontroller = new AccountController();
            Tbl_User tbl_User = new Tbl_User();
            tbl_User.Username = "jenglish";
            tbl_User.Password = "pwd123";
            ActionResult result = accountcontroller.IsValidUser(tbl_User);
            Assert.IsNotNull(result);
        }
    }
}