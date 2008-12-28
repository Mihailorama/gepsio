using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Pure : Decimal
    {
        internal Pure(XmlNode StringRootNode)
            : base(StringRootNode)
        {
        }
    }
}
