using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class MonetaryComplexType : SimpleContentComplexType
    {
        internal MonetaryComplexType(XmlNode ComplexTypeNode, XmlNode SimpleContentNode)
            : base(ComplexTypeNode, SimpleContentNode)
        {
        }
    }
}
