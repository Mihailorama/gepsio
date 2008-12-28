using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class NCName : Name
    {
        internal NCName(XmlNode StringRootNode)
            : base(StringRootNode)
        {
        }
    }
}
