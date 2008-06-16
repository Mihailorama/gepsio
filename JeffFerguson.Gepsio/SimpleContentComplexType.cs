using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class SimpleContentComplexType : ComplexType
    {
        private XmlNode thisSimpleContentNode;

        internal XmlNode SimpleContentNode
        {
            get
            {
                return thisSimpleContentNode;
            }
        }

        internal SimpleContentComplexType(XmlNode ComplexTypeNode, XmlNode SimpleContentNode)
            : base(ComplexTypeNode)
        {
            thisSimpleContentNode = SimpleContentNode;
        }
    }
}
