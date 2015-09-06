using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System.Text;

namespace JeffFerguson.Test.Gepsio
{
    [TestClass]
    public class XbrlDocumentTests
    {
        [TestMethod]
        public void LoadXbrlFromStreamSuccessfully()
        {
            var webClient = new WebClient();
            string readXml = webClient.DownloadString("http://www.xbrl.org/taxonomy/int/fr/ias/ci/pfs/2002-11-15/SampleCompany-2002-11-15.xml");        
            byte[] byteArray = Encoding.ASCII.GetBytes(readXml);
            MemoryStream memStream = new MemoryStream(byteArray);
            var newDoc = new XbrlDocument();
            newDoc.Load(memStream);
            Assert.IsTrue(newDoc.IsValid);
        }

        [TestMethod]
        public void LoadXbrlFromStreamWithRelativeSchema()
        {
            var webClient = new WebClient();
            string readXml = webClient.DownloadString("http://www.sec.gov/Archives/edgar/data/789019/000119312515020351/msft-20141231.xml");
            byte[] byteArray = Encoding.ASCII.GetBytes(readXml);
            MemoryStream memStream = new MemoryStream(byteArray);
            var newDoc = new XbrlDocument();
            newDoc.Load(memStream);
            Assert.IsFalse(newDoc.IsValid);
        }
    }
}
