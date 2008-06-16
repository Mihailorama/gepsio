using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class RestrictedSimpleType : SimpleType
    {
        private XmlNode thisRestrictionNode;

        internal XmlNode RestrictionNode
        {
            get
            {
                return thisRestrictionNode;
            }
        }

        internal RestrictedSimpleType(XmlNode SimpleTypeNode, XmlNode RestrictionNode) : base(SimpleTypeNode)
        {
            thisRestrictionNode = RestrictionNode;
        }
    }
}
