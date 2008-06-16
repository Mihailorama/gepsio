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
    /// Summary description for ConformanceTest
    /// </summary>
    [TestClass]
    public class ConformanceTest
    {
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
        public void ExecuteTestcases()
        {
            XmlDocument ConformanceXmlDocument;

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
            StringBuilder InstanceXmlSourceFullPathBuilder = new StringBuilder();
            InstanceXmlSourceFullPathBuilder.AppendFormat("{0}{1}{2}", TestcaseXmlSourceDirectory, Path.DirectorySeparatorChar, CurrentVariation.Instance);
            string InstanceXmlSourceFullPath = InstanceXmlSourceFullPathBuilder.ToString();

            XbrlDocument NewXbrlDocument = new XbrlDocument();
            XbrlException VariationException = null;

            try
            {
                NewXbrlDocument.Load(InstanceXmlSourceFullPath);
            }
            catch (XbrlException e)
            {
                VariationException = e;
            }
            catch (Exception e)
            {
                // This is a good place to catch non-XBRL exceptions, such as null reference
                // exceptions, during debugging.
                throw e;
            }

            if ((VariationException == null) && (CurrentVariation.ValidityExpected == false))
                AnnounceTestFailure(CurrentVariation);
            if ((VariationException != null) && (CurrentVariation.ValidityExpected == true))
                AnnounceTestFailure(CurrentVariation, VariationException);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void AnnounceTestFailure(TestVariation CurrentVariation, Exception VariationException)
        {
            StringBuilder FailMessage = new StringBuilder();

            FailMessage.AppendFormat("Instance: {0} -- ", CurrentVariation.Instance);
            FailMessage.AppendFormat("Name: {0} -- ", CurrentVariation.Name);
            FailMessage.AppendFormat("Description: {0} -- ", CurrentVariation.Description);
            if (VariationException != null)
            {
                FailMessage.AppendFormat("Exception Type: {0} -- ", VariationException.GetType().ToString());
                FailMessage.AppendFormat("Exception Description: {0} -- ", VariationException.Message);
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
