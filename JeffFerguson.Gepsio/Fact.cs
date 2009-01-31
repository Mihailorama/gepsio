using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Fact
    {
        private XmlNode thisFactNode;
        private string thisContextRefName;
        private string thisUnitRefName;
        private string thisPrecisionAttributeValue;
        private int thisPrecision;
        private int thisDecimals;
        private string thisDecimalsAttributeValue;
        private string thisValue;
        private Context thisContextRef;
        private Unit thisUnitRef;
        private XbrlFragment thisParentFragment;
        private Element thisSchemaElement;
        private string thisName;
        private string thisId;
        private bool thisNilSpecified;
        private AnyType thisItemType;
        private XbrlSchema thisSchema;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public Context ContextRef
        {
            get
            {
                return thisContextRef;
            }
            internal set
            {
                thisContextRef = value;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string ContextRefName
        {
            get
            {
                return thisContextRefName;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public Unit UnitRef
        {
            get
            {
                return thisUnitRef;
            }
            internal set
            {
                thisUnitRef = value;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string UnitRefName
        {
            get
            {
                return thisUnitRefName;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public int Precision
        {
            get
            {
                return thisPrecision;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public int Decimals
        {
            get
            {
                return thisDecimals;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public bool PrecisionSpecified
        {
            get
            {
                if (thisPrecisionAttributeValue.Length == 0)
                    return false;
                return true;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public bool DecimalsSpecified
        {
            get
            {
                if (thisDecimalsAttributeValue.Length == 0)
                    return false;
                return true;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Value
        {
            get
            {
                return thisValue;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public decimal NumericValue
        {
            get
            {
                if (this.Type.NumericType == false)
                    return (decimal)0.0;
                if (PrecisionSpecified == true)
                    return this.Type.GetValueAfterApplyingPrecisionTruncation(this.Precision);
                else
                    return this.Type.GetValueAfterApplyingDecimalsTruncation(this.Decimals);
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Namespace
        {
            get
            {
                return thisFactNode.NamespaceURI;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public Element SchemaElement
        {
            get
            {
                return thisSchemaElement;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Name
        {
            get
            {
                return thisName;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Id
        {
            get
            {
                return thisId;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public bool NilSpecified
        {
            get
            {
                return thisNilSpecified;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public AnyType Type
        {
            get
            {
                return thisItemType;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal Fact(XbrlFragment ParentFragment, XmlNode FactNode)
        {
            thisParentFragment = ParentFragment;
            thisFactNode = FactNode;
            thisName = thisFactNode.LocalName;
            thisContextRefName = XmlUtilities.GetAttributeValue(thisFactNode, "contextRef");
            thisUnitRefName = XmlUtilities.GetAttributeValue(thisFactNode, "unitRef");

            thisPrecisionAttributeValue = XmlUtilities.GetAttributeValue(thisFactNode, "precision");
            if (thisPrecisionAttributeValue.Length > 0)
            {
                if (thisPrecisionAttributeValue.Equals("INF") == true)
                    thisPrecision = 0;
                else
                    thisPrecision = Convert.ToInt32(thisPrecisionAttributeValue);
            }

            thisDecimalsAttributeValue = XmlUtilities.GetAttributeValue(thisFactNode, "decimals");
            if (thisDecimalsAttributeValue.Length > 0)
            {
                if (thisDecimalsAttributeValue.Equals("INF") == true)
                    thisDecimals = 0;
                else
                    thisDecimals = Convert.ToInt32(thisDecimalsAttributeValue);
            }

            thisId = XmlUtilities.GetAttributeValue(thisFactNode, "id");
            thisNilSpecified = false;
            string NilValue = XmlUtilities.GetAttributeValue(thisFactNode, "http://www.w3.org/2001/XMLSchema-instance", "nil");
            if (NilValue.Equals("true", StringComparison.CurrentCultureIgnoreCase) == true)
                thisNilSpecified = true;
            GetSchemaElementFromSchema();
            thisValue = thisFactNode.InnerText;
            if (SchemaElement.SubstitutionGroup == Element.ElementSubstitutionGroup.Item)
            {
                SetItemType(SchemaElement.TypeName);
                thisItemType.ValueAsString = thisValue;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal decimal GetValueAfterApplyingTruncation()
        {
            if (PrecisionSpecified == true)
                return thisItemType.GetValueAfterApplyingPrecisionTruncation(this.Precision);
            else
                return thisItemType.GetValueAfterApplyingDecimalsTruncation(this.Decimals);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void GetSchemaElementFromSchema()
        {
            foreach (XbrlSchema CurrentSchema in thisParentFragment.Schemas)
            {
                if (CurrentSchema.TargetNamespace == this.Namespace)
                {
                    thisSchema = CurrentSchema;
                }
            }
            if (thisSchema == null)
                throw new NotImplementedException();
            foreach (Element CurrentElement in thisSchema.Elements)
            {
                if (CurrentElement.Name == thisName)
                    thisSchemaElement = CurrentElement;
            }
            if (thisSchemaElement == null)
                throw new NotImplementedException();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetItemType(string ItemTypeValue)
        {
            thisItemType = null;
            thisItemType = AnyType.CreateType(ItemTypeValue);
            if (thisItemType == null)
            {
                if (thisSchema != null)
                {
                    thisItemType = (AnyType)(thisSchema.GetSimpleType(GetLocalName(ItemTypeValue)));
                }
            }
            if (thisItemType == null)
            {
                if (thisSchema != null)
                {
                    thisItemType = thisSchema.GetComplexType(GetLocalName(ItemTypeValue));
                }
            }
            if (thisItemType == null)
            {
                string MessageFormat = AssemblyResources.GetName("InvalidElementItemType");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, thisSchema.Path, ItemTypeValue, thisName);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private string GetLocalName(string FullName)
        {
            if (FullName == null)
                throw new NotSupportedException("null full names not supported in Fact.GetLocalName()");
            int ColonIndex = FullName.IndexOf(':');
            if (ColonIndex == -1)
                return FullName;
            return FullName.Substring(ColonIndex + 1);
        }

        //------------------------------------------------------------------------------------
        // Validates a fact.
        //
        // If the fact is associated with a data type, and it should be, then hand the fact
        // off to the data type so that the data type can validate the fact. Some data types
        // have specific requirements for facts that must be checked. For example, monetary
        // types require that their facts have units that are part of the ISO 4217 namespace
        // (http://www.xbrl.org/2003/iso4217). This is checked by the datatype.
        //------------------------------------------------------------------------------------
        internal void Validate()
        {
            //if (this.SchemaElement.Type != null)
            //    this.SchemaElement.Type.ValidateFact(this);
            if (this.Type != null)
                this.Type.ValidateFact(this);
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Context Equal (c-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool ContextEquals(Fact OtherFact)
        {
            if(Object.ReferenceEquals(this.ContextRef, OtherFact.ContextRef) == true)
                return true;
            return this.ContextRef.StructureEquals(OtherFact.ContextRef);
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Parent Equal (p-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool ParentEquals(Fact OtherFact)
        {
            if (thisFactNode == null)
                return false;
            return thisFactNode.ParentEquals(OtherFact.thisFactNode);
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Unit Equal (u-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool UnitEquals(Fact OtherFact)
        {
            if (OtherFact == null)
                return false;
            if (this.UnitRef == null)
                return true;
            if (OtherFact.UnitRef == null)
                return true;
            return this.UnitRef.StructureEquals(OtherFact.UnitRef);
        }
    }
}
