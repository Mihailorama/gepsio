using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JeffFerguson.Test.Gepsio
{
    /// <summary>
    /// Tests written against bugs filed in work items.
    /// </summary>
    [TestClass]
    public class RoleTypeTests
    {
        [TestMethod]
        public void GetRoleTypeTest()
        {
            var NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\RoleType\tgt-20130202.xml");
            var balanceSheetRoleType = NewXbrlDocument.GetRoleType("BalanceSheet"); 
            Assert.IsNotNull(balanceSheetRoleType);
        }

        [TestMethod]
        public void GetCalculationLinkFromRoleTypeTest()
        {
            var NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\RoleType\tgt-20130202.xml");
            var balanceSheetRoleType = NewXbrlDocument.GetRoleType("BalanceSheet");
            var balanceSheetCalculationLink = NewXbrlDocument.GetCalculationLink(balanceSheetRoleType);
            Assert.IsNotNull(balanceSheetCalculationLink);
        }
    }
}
