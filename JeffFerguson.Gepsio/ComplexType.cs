using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class ComplexType
    {
        private XmlNode thisComplexTypeNode;
        private string thisName;

        public string Name
        {
            get
            {
                return thisName;
            }
        }

        internal ComplexType(XmlNode ComplexTypeNode)
        {
            thisComplexTypeNode = ComplexTypeNode;
            thisName = XmlUtilities.GetAttributeValue(ComplexTypeNode, "name");
        }

        internal static ComplexType CreateComplexType(XmlNode ComplexTypeNode)
        {
            foreach (XmlNode CurrentChildNode in ComplexTypeNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("simpleContent") == true)
                    return CreateComplexTypeWithSimpleContent(CurrentChildNode);
            }
            return null;
        }

        private static ComplexType CreateComplexTypeWithSimpleContent(XmlNode SimpleContentNode)
        {
            foreach (XmlNode CurrentChildNode in SimpleContentNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("restriction") == true)
                    return CreateComplexTypeWithRestriction(CurrentChildNode);
            }
            return null;
        }

        private static ComplexType CreateComplexTypeWithRestriction(XmlNode RestrictionNode)
        {
            string BaseType;

            BaseType = XmlUtilities.GetAttributeValue(RestrictionNode, "base");
            if (BaseType.Equals("xbrli:monetaryItemType") == true)
                return new MonetaryComplexType(RestrictionNode.ParentNode.ParentNode, RestrictionNode.ParentNode);
            throw new NotImplementedException("unsupported complex type with restriction");
        }
    }
}
