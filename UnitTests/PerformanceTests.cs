using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace JeffFerguson.Test.Gepsio
{
    /// <summary>
    /// Unit tests measuring various aspects of Gepsio's execution performance.
    /// </summary>
    [TestClass]
    public class PerformanceTests
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// Load times (in seconds) for Gepsio 2.1.0.6 (Sep 2012 CTP): 24.0112276, 26.5228912, 22.4193740
        /// </item>
        /// <item>
        /// Load times (in seconds) for Gepsio 2.1.0.9 (May 2015 CTP): 10.2193900, 07.3005537, 12.5917571
        /// </item>
        /// </list>
        /// </remarks>
        [TestMethod]
        public void aig20130630()
        {
            var xbrlDoc = new XbrlDocument();
            var timeBeforeLoad = System.DateTime.Now;
            xbrlDoc.Load(GetRelativePathForDocument("aig-20130630.xml"));
            var timeAfterLoad = System.DateTime.Now;
            var loadTime = timeAfterLoad - timeBeforeLoad;
            Assert.IsTrue(xbrlDoc.IsValid);
        }

        private string GetRelativePathForDocument(string DocFilename)
        {
            var RelativePath = new StringBuilder();
            RelativePath.AppendFormat("..\\..\\..\\JeffFerguson.Test.Gepsio\\PerformanceTests\\aig20130630\\{0}", DocFilename);
            return RelativePath.ToString();
        }
    }
}
