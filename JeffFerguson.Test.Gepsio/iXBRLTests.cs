using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JeffFerguson.Test.Gepsio
{
	[TestClass]
	public class iXBRLTests
	{
		[TestMethod]
		[Ignore] // iXBRL unsupported at this time
		public void UKGAAP2009Test()
		{
			var doc = new XbrlDocument();
			doc.Load(@"..\..\..\JeffFerguson.Test.Gepsio\ixbrl-samples\UKGAAP2009.xml");
			Assert.IsNotNull(doc.XbrlFragments);
			Assert.AreNotEqual<int>(0, doc.XbrlFragments.Count, "No fragments found in document.");
		}
	}
}
