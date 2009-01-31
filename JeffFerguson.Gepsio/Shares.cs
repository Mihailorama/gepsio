using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Shares : Decimal
    {
        internal Shares(XmlNode StringRootNode) : base(StringRootNode)
        {
        }
    }
}
