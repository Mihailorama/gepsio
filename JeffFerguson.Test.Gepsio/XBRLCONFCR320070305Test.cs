using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;

namespace JeffFerguson.Test.Gepsio
{
	/// <summary>
	/// This class executes the XBRL-CONF-CR3-2007-03-05 conformance test suite.
	/// </summary>
	/// <remarks>
	/// The test suite is driven by a document in the root of the conformance suite
	/// folder named "xbrl.xml". The root document is an XML document that contains
	/// a set of &lt;testcase&gt; elements, each of which references a separate
	/// test case document.
	/// </remarks>
	[TestClass]
	public class XBRLCONFCR320070305Test
	{
		private int thisTestsPassed;

		public XBRLCONFCR320070305Test()
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
			thisTestsPassed = 0;
			var conformanceXmlSource = ConfigurationManager.AppSettings["ConformanceXml"];
			var conformanceXmlSourcePath = Path.GetDirectoryName(conformanceXmlSource);
			var conformanceXmlDocument = new XmlDocument();
			conformanceXmlDocument.Load(conformanceXmlSource);
			var TestcaseNodes = conformanceXmlDocument.SelectNodes("//testcase");
			foreach (XmlNode TestcaseNode in TestcaseNodes)
			{
				ExecuteTestcase(conformanceXmlSourcePath, TestcaseNode);
			}
		}

		/// <summary>
		/// Executes the test case referenced in the supplied test case node.
		/// </summary>
		/// <param name="ConformanceXmlSourcePath">
		/// The path to the conformance suite.
		/// </param>
		/// <param name="TestcaseNode">
		/// A reference to one of the &lt;testcase&gt; elements in the root test suiteb ocument.
		/// </param>
		private void ExecuteTestcase(string ConformanceXmlSourcePath, XmlNode TestcaseNode)
		{
			var uriAttribute = TestcaseNode.Attributes["uri"];
			var testcaseXmlSource = uriAttribute.Value;
			var testcaseXmlSourceFullPathBuilder = new StringBuilder();
			testcaseXmlSourceFullPathBuilder.AppendFormat("{0}{1}{2}", ConformanceXmlSourcePath, Path.DirectorySeparatorChar, testcaseXmlSource);
			var testcaseXmlSourceFullPath = testcaseXmlSourceFullPathBuilder.ToString();
			var testcaseXmlSourceDirectory = Path.GetDirectoryName(testcaseXmlSourceFullPath);
			var testcaseXmlDocument = new XmlDocument();
			testcaseXmlDocument.Load(testcaseXmlSourceFullPath);
			var variationNodes = testcaseXmlDocument.SelectNodes("//variation");
			foreach (XmlNode VariationNode in variationNodes)
			{
				ExecuteVariation(testcaseXmlSourceDirectory, VariationNode);
			}
		}

		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		private void ExecuteVariation(string TestcaseXmlSourceDirectory, XmlNode VariationNode)
		{
			var currentVariation = new XBRLCONFCR320070305TestVariation(VariationNode);
			if (string.IsNullOrEmpty(currentVariation.Instance) == true)
				return;

			var instanceXmlSourceFullPathBuilder = new StringBuilder();
			instanceXmlSourceFullPathBuilder.AppendFormat("{0}{1}{2}", TestcaseXmlSourceDirectory, Path.DirectorySeparatorChar, currentVariation.Instance);
			var instanceXmlSourceFullPath = instanceXmlSourceFullPathBuilder.ToString();

			var newXbrlDocument = new XbrlDocument();
			XbrlException VariationException = null;

			try
			{
				newXbrlDocument.Load(instanceXmlSourceFullPath);
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

			if ((VariationException == null) && (currentVariation.ValidityExpected == false))
				AnnounceTestFailure(currentVariation);
			if ((VariationException != null) && (currentVariation.ValidityExpected == true))
				AnnounceTestFailure(currentVariation, VariationException);
			thisTestsPassed++;
		}

		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		private void AnnounceTestFailure(XBRLCONFCR320070305TestVariation CurrentVariation, Exception VariationException)
		{
			var failMessage = new StringBuilder();

			failMessage.AppendFormat("Instance: {0}{1}", CurrentVariation.Instance, Environment.NewLine);
			failMessage.AppendFormat("Name: {0}{1}", CurrentVariation.Name, Environment.NewLine);
			failMessage.AppendFormat("Description: {0}", CurrentVariation.Description);
			if (VariationException != null)
			{
				failMessage.AppendFormat("{0}Exception Type: {1}{2}", Environment.NewLine, VariationException.GetType().ToString(), Environment.NewLine);
				failMessage.AppendFormat("Exception Description: {0}", VariationException.Message);
			}
			Assert.Fail(failMessage.ToString());
		}

		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		private void AnnounceTestFailure(XBRLCONFCR320070305TestVariation CurrentVariation)
		{
			AnnounceTestFailure(CurrentVariation, null);
		}
	}
}
