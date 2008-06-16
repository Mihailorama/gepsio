using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class SimpleType
    {
        private XmlNode thisSimpleTypeNode;
        private string thisName;

        internal XmlNode SimpleTypeNode
        {
            get
            {
                return thisSimpleTypeNode;
            }
        }

        public string Name
        {
            get
            {
                return thisName;
            }
        }

        internal SimpleType(XmlNode SimpleTypeRootNode)
        {
            thisSimpleTypeNode = SimpleTypeRootNode;
            thisName = XmlUtilities.GetAttributeValue(thisSimpleTypeNode, "name");
        }

        internal static SimpleType CreateSimpleType(XmlNode SimpleTypeNode)
        {
            foreach (XmlNode CurrentChildNode in SimpleTypeNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("restriction") == true)
                    return CreateSimpleTypeContainingRestriction(SimpleTypeNode, CurrentChildNode);
            }
            return null;
        }

        private static SimpleType CreateSimpleTypeContainingRestriction(XmlNode SimpleTypeNode, XmlNode RestrictionNode)
        {
            string BaseValue = RestrictionNode.Attributes["base"].Value;
            switch (BaseValue)
            {
                case "token":
                    return new TokenSimpleType(SimpleTypeNode, RestrictionNode);
                case "string":
                    return new StringSimpleType(SimpleTypeNode, RestrictionNode);
                default:
                    throw new NotImplementedException("unsupported simple type");
            }
        }
    }
}
