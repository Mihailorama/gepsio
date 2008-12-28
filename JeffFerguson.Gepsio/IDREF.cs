using System.Xml;

namespace JeffFerguson.Gepsio
{
    internal class IDREF : NCName
    {
        internal IDREF(XmlNode StringRootNode)
            : base(StringRootNode)
        {
        }
    }
}
