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
        private AnyType thisSimpleContentType;
        private string thisValueAsString;
        private AttributeGroup thisAttributeGroup;

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

        //--------------------------------------------------------------------------------------------------------
        // This constructor is used to construct user-defined complex types defined in XBRL schemas.
        //--------------------------------------------------------------------------------------------------------
        internal ComplexType(XmlNode ComplexTypeNode)
        {
            thisAttributeGroup = null;
            thisComplexTypeNode = ComplexTypeNode;
            thisName = XmlUtilities.GetAttributeValue(ComplexTypeNode, "name");
            thisSimpleContentType = null;
            foreach (XmlNode CurrentChildNode in ComplexTypeNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("simpleContent") == true)
                    thisSimpleContentType = new SimpleType(CurrentChildNode);
            }
        }

        //--------------------------------------------------------------------------------------------------------
        // This constructor is used to construct built-in complex types defined in the XBRL specification.
        //--------------------------------------------------------------------------------------------------------
        internal ComplexType(string Name, AnyType BaseSimpleType, AttributeGroup AttrGroup)
        {
            thisAttributeGroup = AttrGroup;
            thisComplexTypeNode = null;
            thisName = Name;
            thisSimpleContentType = BaseSimpleType;
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal override void ValidateFact(Fact FactToValidate)
        {
            if (thisSimpleContentType != null)
                thisSimpleContentType.ValidateFact(FactToValidate);
        }
    }
}
