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
        /// Load times (in milliseconds) for Gepsio 2.1.0.6 (Sep 2012 CTP): 240112276, 265228912, 224193740
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
