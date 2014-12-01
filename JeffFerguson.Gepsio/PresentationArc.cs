using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "presentationArc" as defined in the http://www.xbrl.org/2003/linkbase namespace.
    /// </summary>
    public class PresentationArc : XlinkNode
    {
        public double Order
        {
            get;
            private set;
        }

        internal PresentationArc(INode presentationArcNode) : base(presentationArcNode)
        {
            var orderAsString = presentationArcNode.GetAttributeValue("order");
            double orderParsedValue;
            if (double.TryParse(orderAsString, out orderParsedValue) == true)
                Order = orderParsedValue;
        }
    }
}
