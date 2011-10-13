using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Configuration;
using System.IO;
using JeffFerguson.Gepsio;

namespace JeffFerguson.Test.Gepsio
{
    /// <summary>
    /// This class executes the XBRL-CONF-CR3-2007-03-05 conformance test suite.
    /// </summary>
    /// <remarks>
    /// The test suite is driven by a document in the root
    /// </remarks>
    [TestClass]
    public class ConformanceTest
    {
        private int thisTestsPassed;

        public ConformanceTest()
        {
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
        [Description("XBRL-CONF-CR3-2007-03-05")]
        public void ExecuteXBRLCONFCR320070305Testcases()
        {
            XmlDocument ConformanceXmlDocument;

            thisTestsPassed = 0;
            string ConformanceXmlSource = ConfigurationManager.AppSettings["ConformanceXml"];
            string ConformanceXmlSourcePath = Path.GetDirectoryName(ConformanceXmlSource);
            ConformanceXmlDocument = new XmlDocument();
            ConformanceXmlDocument.Load(ConformanceXmlSource);
            XmlNodeList TestcaseNodes = ConformanceXmlDocument.SelectNodes("//testcase");
            foreach (XmlNode TestcaseNode in TestcaseNodes)
            {
                ExecuteTestcase(ConformanceXmlSourcePath, TestcaseNode);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ExecuteTestcase(string ConformanceXmlSourcePath, XmlNode TestcaseNode)
        {
            XmlAttribute UriAttribute = TestcaseNode.Attributes["uri"];
            string TestcaseXmlSource = UriAttribute.Value;
            StringBuilder TestcaseXmlSourceFullPathBuilder = new StringBuilder();
            TestcaseXmlSourceFullPathBuilder.AppendFormat("{0}{1}{2}", ConformanceXmlSourcePath, Path.DirectorySeparatorChar, TestcaseXmlSource);
            string TestcaseXmlSourceFullPath = TestcaseXmlSourceFullPathBuilder.ToString();
            string TestcaseXmlSourceDirectory = Path.GetDirectoryName(TestcaseXmlSourceFullPath);
            XmlDocument TestcaseXmlDocument = new XmlDocument();
            TestcaseXmlDocument.Load(TestcaseXmlSourceFullPath);
            XmlNodeList VariationNodes = TestcaseXmlDocument.SelectNodes("//variation");
            foreach (XmlNode VariationNode in VariationNodes)
            {
                ExecuteVariation(TestcaseXmlSourceDirectory, VariationNode);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ExecuteVariation(string TestcaseXmlSourceDirectory, XmlNode VariationNode)
        {
            TestVariation CurrentVariation = new TestVariation(VariationNode);
            if (string.IsNullOrEmpty(CurrentVariation.Instance) == true)
                return;

            StringBuilder InstanceXmlSourceFullPathBuilder = new StringBuilder();
            InstanceXmlSourceFullPathBuilder.AppendFormat("{0}{1}{2}", TestcaseXmlSourceDirectory, Path.DirectorySeparatorChar, CurrentVariation.Instance);
            string InstanceXmlSourceFullPath = InstanceXmlSourceFullPathBuilder.ToString();

            XbrlDocument NewXbrlDocument = new XbrlDocument();
            XbrlException VariationException = null;

            try
            {
                NewXbrlDocument.Load(InstanceXmlSourceFullPath);
            }
            catch (XbrlException xbrle)
            {
                VariationException = xbrle;
            }
            catch (Exception e)
            {
                // This is a good place to catch non-XBRL exceptions, such as null reference
                // exceptions, during debugging.
                throw;
            }

            if ((VariationException == null) && (CurrentVariation.ValidityExpected == false))
                AnnounceTestFailure(CurrentVariation);
            if ((VariationException != null) && (CurrentVariation.ValidityExpected == true))
                AnnounceTestFailure(CurrentVariation, VariationException);
            thisTestsPassed++;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void AnnounceTestFailure(TestVariation CurrentVariation, Exception VariationException)
        {
            StringBuilder FailMessage = new StringBuilder();

            FailMessage.AppendFormat("Instance: {0}{1}", CurrentVariation.Instance, Environment.NewLine);
            FailMessage.AppendFormat("Name: {0}{1}", CurrentVariation.Name, Environment.NewLine);
            FailMessage.AppendFormat("Description: {0}", CurrentVariation.Description);
            if (VariationException != null)
            {
                FailMessage.AppendFormat("{0}Exception Type: {1}{2}", Environment.NewLine, VariationException.GetType().ToString(), Environment.NewLine);
                FailMessage.AppendFormat("Exception Description: {0}", VariationException.Message);
            }
            Assert.Fail(FailMessage.ToString());
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void AnnounceTestFailure(TestVariation CurrentVariation)
        {
            AnnounceTestFailure(CurrentVariation, null);
        }
    }
}
