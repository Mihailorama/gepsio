using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class ID : NCName
    {
        internal ID(XmlNode StringRootNode)
            : base(StringRootNode)
        {
        }
    }
}
