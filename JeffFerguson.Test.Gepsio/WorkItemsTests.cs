using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\WorkItemsInput\WorkItem3828\cmi-20081231.xml");
        }

        /// <summary>
        /// Work item 3903 notes that multiple namespace URIs can exist in an XML schema, and early versions of Gepsio didn't support that.
        /// </summary>
        [TestMethod]
        public void WorkItem3903Test()
        {
            var NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\WorkItemsInput\WorkItem3903\WorkItem3903.xml");
        }

		/// <summary>
		/// Work item 9401 notes that Gepsio throws a Null Reference Exception in the Qualified Name code when the associated
		/// document is loaded.
		/// </summary>
		[TestMethod]
		public void WorkItem9401Test()
		{
			var NewXbrlDocument = new XbrlDocument();
			NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\WorkItemsInput\WorkItem9401\amzn-20120331.xml");
		}

        /// <summary>
        /// Work item 9571 notes that the Nov 2011 CTP has no support for schema role types.
        /// </summary>
        [TestMethod]
        public void WorkItem9571Test()
        {
            var xbrlDocument = new XbrlDocument();
            xbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\WorkItemsInput\WorkItem9571\Sample-Instance-Proof.xml");
            Assert.AreEqual<int>(1, xbrlDocument.XbrlFragments.Count);
            var firstFragment = xbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, firstFragment.Schemas.Count);
            var firstSchema = firstFragment.Schemas[0];
            Assert.AreEqual<int>(60, firstSchema.RoleTypes.Count);
        }
    }
}
