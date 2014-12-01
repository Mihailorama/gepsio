using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "presentationArc" as defined in the http://www.xbrl.org/2003/linkbase namespace.
    /// </summary>
    public class PresentationArc : XlinkNode
    {
        internal PresentationArc(INode presentationArcNode) : base(presentationArcNode)
        {
        }
    }
}
