using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace JeffFerguson.Test.Gepsio
{
    /// <summary>
    /// Unit tests for various aspects of Gepsio's presentable fact tree code.
    /// </summary>
    [TestClass]
    public class PresentableFactTreeTests
    {
        [TestMethod]
        public void PresentableTreeForAig20130630()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(GetRelativePathForDocument("aig-20130630.xml"));
            var firstFragment = xbrlDoc.XbrlFragments[0];
            var tree = firstFragment.GetPresentableFactTree();
            Assert.IsNotNull(tree);
        }

        private string GetRelativePathForDocument(string DocFilename)
        {
            var RelativePath = new StringBuilder();
            RelativePath.AppendFormat("..\\..\\..\\JeffFerguson.Test.Gepsio\\PerformanceTests\\aig20130630\\{0}", DocFilename);
            return RelativePath.ToString();
        }
    }
}
