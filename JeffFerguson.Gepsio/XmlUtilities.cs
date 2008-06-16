using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    internal static class XmlUtilities
    {
        internal static string GetAttributeValue(XmlNode Node, string AttributeName)
        {
            XmlAttribute Attr = Node.Attributes[AttributeName];
            if (Attr == null)
                return string.Empty;
            return Attr.Value;
        }
    }
}
