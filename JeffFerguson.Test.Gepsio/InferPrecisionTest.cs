using Microsoft.VisualStudio.TestTools.UnitTesting;
using JeffFerguson.Gepsio;

namespace JeffFerguson.Test.Gepsio
{
    /// <summary>
    /// The InferPrecisionTest class tests the precision inferrence code for XBRL facts. In the RECOMMENDATION 2003-12-31 + Corrected Errata
    /// 2008-07-02 version of the XBRL spec, the section on precision inferrence for XBRL facts can be found in section 4.6.6. Each test method
    /// tests the values shown in each row of the table for Example 13 on Page 53.
    /// </summary>
    [TestClass]
    public class InferPrecisionTest
    {
        /// <summary>
        /// Tests the example given in Row 1 of the table in Example 13. The row lists the following information:
        /// LEXICAL REPRESENTATION: 123
        /// DECIMALS VALUE: 2
        /// INFERRED PRECISION: 5
        /// </summary>
        [TestMethod]
        public void Example13Row1()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\InferPrecisionTestDocuments\Example13Row1.xbrl");
            Assert.AreEqual<int>(1, NewXbrlDocument.XbrlFragments.Count, "No XBRL fragments found.");
            XbrlFragment FirstFragment = NewXbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, FirstFragment.Facts.Count, "No facts found in fragment.");
            Item FirstFact = FirstFragment.Facts[0] as Item;
            Assert.IsTrue(FirstFact.DecimalsSpecified);
            Assert.IsFalse(FirstFact.PrecisionSpecified);
            Assert.IsTrue(FirstFact.PrecisionInferred);
            Assert.AreEqual<int>(5, FirstFact.Precision);
        }

        /// <summary>
        /// Tests the example given in Row 2 of the table in Example 13. The row lists the following information:
        /// LEXICAL REPRESENTATION: 123.4567
        /// DECIMALS VALUE: 2
        /// INFERRED PRECISION: 5
        /// </summary>
        [TestMethod]
        public void Example13Row2()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\InferPrecisionTestDocuments\Example13Row2.xbrl");
            Assert.AreEqual<int>(1, NewXbrlDocument.XbrlFragments.Count, "No XBRL fragments found.");
            XbrlFragment FirstFragment = NewXbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, FirstFragment.Facts.Count, "No facts found in fragment.");
            Item FirstFact = FirstFragment.Facts[0] as Item;
            Assert.IsTrue(FirstFact.DecimalsSpecified);
            Assert.IsFalse(FirstFact.PrecisionSpecified);
            Assert.IsTrue(FirstFact.PrecisionInferred);
            Assert.AreEqual<int>(5, FirstFact.Precision);
        }

        /// <summary>
        /// Tests the example given in Row 3 of the table in Example 13. The row lists the following information:
        /// LEXICAL REPRESENTATION: 123e5
        /// DECIMALS VALUE: -3
        /// INFERRED PRECISION: 5
        /// </summary>
        [TestMethod]
        public void Example13Row3()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\InferPrecisionTestDocuments\Example13Row3.xbrl");
            Assert.AreEqual<int>(1, NewXbrlDocument.XbrlFragments.Count, "No XBRL fragments found.");
            XbrlFragment FirstFragment = NewXbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, FirstFragment.Facts.Count, "No facts found in fragment.");
            Item FirstFact = FirstFragment.Facts[0] as Item;
            Assert.IsTrue(FirstFact.DecimalsSpecified);
            Assert.IsFalse(FirstFact.PrecisionSpecified);
            Assert.IsTrue(FirstFact.PrecisionInferred);
            Assert.AreEqual<int>(5, FirstFact.Precision);
        }

        /// <summary>
        /// Tests the example given in Row 4 of the table in Example 13. The row lists the following information:
        /// LEXICAL REPRESENTATION: 123.45e5
        /// DECIMALS VALUE: -3
        /// INFERRED PRECISION: 5
        /// </summary>
        [TestMethod]
        public void Example13Row4()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\InferPrecisionTestDocuments\Example13Row4.xbrl");
            Assert.AreEqual<int>(1, NewXbrlDocument.XbrlFragments.Count, "No XBRL fragments found.");
            XbrlFragment FirstFragment = NewXbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, FirstFragment.Facts.Count, "No facts found in fragment.");
            Item FirstFact = FirstFragment.Facts[0] as Item;
            Assert.IsTrue(FirstFact.DecimalsSpecified);
            Assert.IsFalse(FirstFact.PrecisionSpecified);
            Assert.IsTrue(FirstFact.PrecisionInferred);
            Assert.AreEqual<int>(5, FirstFact.Precision);
        }

        /// <summary>
        /// Tests the example given in Row 5 of the table in Example 13. The row lists the following information:
        /// LEXICAL REPRESENTATION: 0.1e-2
        /// DECIMALS VALUE: 5
        /// INFERRED PRECISION: 3
        /// </summary>
        [TestMethod]
        public void Example13Row5()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\InferPrecisionTestDocuments\Example13Row5.xbrl");
            Assert.AreEqual<int>(1, NewXbrlDocument.XbrlFragments.Count, "No XBRL fragments found.");
            XbrlFragment FirstFragment = NewXbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, FirstFragment.Facts.Count, "No facts found in fragment.");
            Item FirstFact = FirstFragment.Facts[0] as Item;
            Assert.IsTrue(FirstFact.DecimalsSpecified);
            Assert.IsFalse(FirstFact.PrecisionSpecified);
            Assert.IsTrue(FirstFact.PrecisionInferred);
            Assert.AreEqual<int>(3, FirstFact.Precision);
        }

        /// <summary>
        /// Tests the example given in Row 6 of the table in Example 13. The row lists the following information:
        /// LEXICAL REPRESENTATION: 0.001E-2
        /// DECIMALS VALUE: 5
        /// INFERRED PRECISION: 1
        /// </summary>
        [TestMethod]
        public void Example13Row6()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\InferPrecisionTestDocuments\Example13Row6.xbrl");
            Assert.AreEqual<int>(1, NewXbrlDocument.XbrlFragments.Count, "No XBRL fragments found.");
            XbrlFragment FirstFragment = NewXbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, FirstFragment.Facts.Count, "No facts found in fragment.");
            Item FirstFact = FirstFragment.Facts[0] as Item;
            Assert.IsTrue(FirstFact.DecimalsSpecified);
            Assert.IsFalse(FirstFact.PrecisionSpecified);
            Assert.IsTrue(FirstFact.PrecisionInferred);
            Assert.AreEqual<int>(1, FirstFact.Precision);
        }

        /// <summary>
        /// Tests the example given in Row 7 of the table in Example 13. The row lists the following information:
        /// LEXICAL REPRESENTATION: 0.001e-3
        /// DECIMALS VALUE: 4
        /// INFERRED PRECISION: 0
        /// </summary>
        [TestMethod]
        public void Example13Row7()
        {
            XbrlDocument NewXbrlDocument = new XbrlDocument();
            NewXbrlDocument.Load(@"..\..\..\JeffFerguson.Test.Gepsio\InferPrecisionTestDocuments\Example13Row7.xbrl");
            Assert.AreEqual<int>(1, NewXbrlDocument.XbrlFragments.Count, "No XBRL fragments found.");
            XbrlFragment FirstFragment = NewXbrlDocument.XbrlFragments[0];
            Assert.AreEqual<int>(1, FirstFragment.Facts.Count, "No facts found in fragment.");
            Item FirstFact = FirstFragment.Facts[0] as Item;
            Assert.IsTrue(FirstFact.DecimalsSpecified);
            Assert.IsFalse(FirstFact.PrecisionSpecified);
            Assert.IsTrue(FirstFact.PrecisionInferred);
            Assert.AreEqual<int>(0, FirstFact.Precision);
        }
    }
}
