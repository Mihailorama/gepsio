using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class ComplexType : AnyType
    {
        private XmlNode thisComplexTypeNode;
        private string thisName;
        private SimpleType thisSimpleContentType;
        private string thisValueAsString;

        public string Name
        {
            get
            {
                return thisName;
            }
        }

        public override string ValueAsString
        {
            get
            {
                return thisValueAsString;
            }
            set
            {
                thisValueAsString = value;
            }
        }

        internal ComplexType(XmlNode ComplexTypeNode)
        {
            thisComplexTypeNode = ComplexTypeNode;
            thisName = XmlUtilities.GetAttributeValue(ComplexTypeNode, "name");
            thisSimpleContentType = null;
            foreach (XmlNode CurrentChildNode in ComplexTypeNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("simpleContent") == true)
                    thisSimpleContentType = new SimpleType(CurrentChildNode);
            }
        }

        internal override void ValidateFact(Fact FactToValidate)
        {
            if (thisSimpleContentType != null)
                thisSimpleContentType.ValidateFact(FactToValidate);
        }
    }
}
