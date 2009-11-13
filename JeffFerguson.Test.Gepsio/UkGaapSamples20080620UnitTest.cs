using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace JeffFerguson.Test.Gepsio
{
    /// <summary>
    /// Summary description for UkGaapSamples20080620UnitTest
    /// </summary>
    [TestClass]
    public class UkGaapSamples20080620UnitTest
    {
        public UkGaapSamples20080620UnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void LoadAccountingPoliciesDimensionXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("accounting-policies-dimension.xml"));
        }

        [TestMethod]
        public void LoadBalanceSheetIndividualForeignCurrencyXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("balance-sheet-individual-foreign-currency.xml"));
        }

        [TestMethod]
        public void LoadBalanceSheetIndividualSchedule4Format1Xbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("balance-sheet-individual-schedule-4-format-1.xml"));
        }

        [TestMethod]
        public void LoadCashFlowStatementGroupXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("cash-flow-statement-group.xml"));
        }

        [TestMethod]
        public void LoadCashFlowStatementCopyXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("cash-flow-statement - Copy.xml"));
        }

        [TestMethod]
        public void LoadDetailedProfitLossAccountDimensionXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("detailed-profit-loss-account-dimension.xml"));
        }

        [TestMethod]
        public void LoadDetailedProfitLossAccountXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("detailed-profit-loss-account.xml"));
        }

        [TestMethod]
        public void LoadHistoricalCostProfitsLossesXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("historical-cost-profits-losses.xml"));
        }

        [TestMethod]
        public void LoadNotePensionFrs17AmendmentsXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("note-pension-frs17-amendments.xml"));
        }

        [TestMethod]
        public void LoadNotePensionXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("note-pension.xml"));
        }

        [TestMethod]
        public void LoadNoteSegmentReportingDimensionExtensionXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("note-segment-reporting-dimension-extension.xml"));
        }

        [TestMethod]
        public void LoadNoteTangibleFixedAssetsDimensionXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("note-tangible-fixed-assets-dimension.xml"));
        }

        [TestMethod]
        public void LoadNoteTaxationXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("note-taxation.xml"));
        }

        [TestMethod]
        public void LoadProfitLossAccountGroupSchedule4Format1DimensionXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("profit-loss-account-group-schedule-4-format-1-dimension.xml"));
        }

        [TestMethod]
        public void LoadProfitLossAccountIndividualSchedule4Format1Xbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("profit-loss-account-individual-schedule-4-format-1.xml"));
        }

        [TestMethod]
        public void LoadStatementTotalRecognisedGainsLossesDimensionXbrl()
        {
            XbrlDocument XbrlDoc = new XbrlDocument();
            XbrlDoc.Load(GetRelativePathForDocument("statement-total-recognised-gains-losses-dimension.xml"));
        }

        private string GetRelativePathForDocument(string DocFilename)
        {
            StringBuilder RelativePath = new StringBuilder();
            RelativePath.AppendFormat("..\\..\\..\\JeffFerguson.Test.Gepsio\\uk-gaap-samples-2008-06-20\\samples\\{0}", DocFilename);
            return RelativePath.ToString();
        }
    }
}
