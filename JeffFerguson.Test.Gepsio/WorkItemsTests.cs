using Microsoft.VisualStudio.TestTools.UnitTesting;
using JeffFerguson.Gepsio;

namespace JeffFerguson.Test.Gepsio
{
    /// <summary>
    /// Tests written against bugs filed in work items.
    /// </summary>
    [TestClass]
    public class WorkItemsTests
    {
        /// <summary>
        /// Work item 3828 contains several calculation arcs. Each of the calculation arcs uses a decimal number as the value of the "order"
        /// attribute. Section 3.5.3.9 of the XBRL 2.1 spec does indeed specify that the "order" attribute value is of type "decimal". Early versions
        /// of Gepsio assumed that this value was of an integer type; those early versions of Gepsio would throw an exception when trying
        /// to parse the decimal-based "order" attributes as an integer.
        /// </summary>
        [TestMethod]
        public void WorkItem3828Test()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\WorkItemsInput\WorkItem3828\cmi-20081231.xml");
        }

        /// <summary>
        /// Work item 3903 notes that multiple namespace URIs can exist in an XML schema, and early versions of Gepsio didn't support that.
        /// </summary>
        [TestMethod]
        public void WorkItem3903Test()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\WorkItemsInput\WorkItem3903\WorkItem3903.xml");
        }
    }
}
