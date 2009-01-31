using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class SimpleType : AnySimpleType
    {
        private XmlNode thisSimpleTypeNode;
        private string thisName;
        private AnyType thisRestrictionType;

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
            foreach (XmlNode CurrentChildNode in SimpleTypeNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("restriction") == true)
                    CreateRestrictionType(CurrentChildNode);
            }
        }

        private void CreateRestrictionType(XmlNode CurrentChildNode)
        {
            string BaseValue = CurrentChildNode.Attributes["base"].Value;
            thisRestrictionType = AnyType.CreateType(BaseValue, CurrentChildNode);
            if (thisRestrictionType == null)
            {
                string MessageFormat = AssemblyResources.GetName("UnsupportedRestrictionBaseSimpleType");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, BaseValue);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        internal override void ValidateFact(Fact FactToValidate)
        {
            if (thisRestrictionType != null)
                thisRestrictionType.ValidateFact(FactToValidate);
        }
    }
}
