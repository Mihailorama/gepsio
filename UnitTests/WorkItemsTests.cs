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
        /// <remarks>
        /// This test must be ignored, as one of the type definitions refers to a base type at an XBRL.US
        /// location which no longer exists.
        /// </remarks>
        [TestMethod]
        [Ignore]
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
        /// <remarks>
        /// This test must be ignored, as one of the type definitions refers to a base type at an XBRL.US
        /// location which no longer exists.
        /// </remarks>
        [TestMethod]
        [Ignore]
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

        /// <summary>
        /// Work item 9612 notes that the Nov 2011 failed to load a document. The actual issue is that the 
        /// schema references are HTTP-based, and not file-based, which confuses the code that calculates the
        /// full path to the referenced schema.
        /// </summary>
        /// <remarks>
        /// This document is invalid because it contains a calculation error. The current validation error
        /// reporting scheme does not provide an easy way to tell if an XBRL exception thrown as the result of
        /// a validation error because of the schemas or because of a calculation. Ideally, this test needs
        /// to say "validation error is OK as long as the schemas loaded". Currently, there is no good way to
        /// write this kind of code. This test should be revisited if and when the validation reporting code is
        /// redesigned (see http://gepsio.blogspot.com/2012/09/gepsio-xbrl-validation-strategies.html for more
        /// information on this possibility).
        /// </remarks>
        [TestMethod]
        [Ignore]
        public void WorkItem9612Test()
        {
            var xbrlDocument = new XbrlDocument();               
            xbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\WorkItemsInput\WorkItem9612\intc-20111231.xml");
            Assert.AreEqual<int>(1, xbrlDocument.XbrlFragments.Count);
            var firstFragment = xbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, firstFragment.Schemas.Count);
            var firstSchema = firstFragment.Schemas[0];
        }
    }
}
