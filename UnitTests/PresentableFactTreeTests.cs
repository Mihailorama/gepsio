using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
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

        [TestMethod]
        public void WalkPresentableTree()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(GetRelativePathForDocument("aig-20130630.xml"));
            var firstFragment = xbrlDoc.XbrlFragments[0];
            var tree = firstFragment.GetPresentableFactTree();
            foreach (var currentNode in tree.TopLevelNodes)
                WalkPresentableTreeNode(currentNode, 0);
        }

        private void WalkPresentableTreeNode(PresentableFactTreeNode node, int depth)
        {
            for (var indent = 0; indent < depth; indent++)
                Debug.Write("    ");
            if (node.NodeFact != null)
            {
                if (node.NodeFact is Item)
                {
                    var nodeItem = node.NodeFact as Item;
                    Debug.Write(nodeItem.Name);
                    Debug.Write(" ");
                    Debug.Write(nodeItem.Value);
                }
            }
            else
                Debug.Write(node.NodeLabel);
            Debug.WriteLine("");
            foreach (var childNode in node.ChildNodes)
                WalkPresentableTreeNode(childNode, depth + 1);
        }

        private string GetRelativePathForDocument(string DocFilename)
        {
            var RelativePath = new StringBuilder();
            RelativePath.AppendFormat("..\\..\\..\\JeffFerguson.Test.Gepsio\\PerformanceTests\\aig20130630\\{0}", DocFilename);
            return RelativePath.ToString();
        }
    }
}
