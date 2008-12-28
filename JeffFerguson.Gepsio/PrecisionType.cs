using System.Xml;

namespace JeffFerguson.Gepsio
{
    internal class PrecisionType : NonNegativeInteger
    {
        internal PrecisionType(XmlNode StringRootNode) : base(StringRootNode)
        {
        }
    }
}
