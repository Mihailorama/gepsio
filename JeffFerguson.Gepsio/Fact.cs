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
        private string thisPrecision;
        private string thisValue;
        private Context thisContextRef;
        private Unit thisUnitRef;
        private XbrlFragment thisParentFragment;
        private Element thisSchemaElement;
        private string thisName;
        private string thisId;

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

        public string ContextRefName
        {
            get
            {
                return thisContextRefName;
            }
        }

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

        public string UnitRefName
        {
            get
            {
                return thisUnitRefName;
            }
        }

        public string Precision
        {
            get
            {
                return thisPrecision;
            }
        }

        public string Value
        {
            get
            {
                return thisValue;
            }
        }

        public string Namespace
        {
            get
            {
                return thisFactNode.NamespaceURI;
            }
        }

        public Element SchemaElement
        {
            get
            {
                if (thisSchemaElement == null)
                    GetSchemaElementFromSchema();
                return thisSchemaElement;
            }
        }

        public string Name
        {
            get
            {
                return thisName;
            }
        }

        public string Id
        {
            get
            {
                return thisId;
            }
        }

        internal Fact(XbrlFragment ParentFragment, XmlNode FactNode)
        {
            thisParentFragment = ParentFragment;
            thisFactNode = FactNode;
            thisName = thisFactNode.LocalName;
            thisContextRefName = XmlUtilities.GetAttributeValue(thisFactNode, "contextRef");
            thisUnitRefName = XmlUtilities.GetAttributeValue(thisFactNode, "unitRef");
            thisPrecision = XmlUtilities.GetAttributeValue(thisFactNode, "precision");
            thisId = XmlUtilities.GetAttributeValue(thisFactNode, "id");
            thisValue = thisFactNode.InnerText;
        }

        private void GetSchemaElementFromSchema()
        {
            XbrlSchema MatchingSchema = null;
            foreach (XbrlSchema CurrentSchema in thisParentFragment.Schemas)
            {
                if (CurrentSchema.TargetNamespace == this.Namespace)
                {
                    MatchingSchema = CurrentSchema;
                }
            }
            if (MatchingSchema == null)
                throw new NotImplementedException();
            foreach (Element CurrentElement in MatchingSchema.Elements)
            {
                if (CurrentElement.Name == thisName)
                    thisSchemaElement = CurrentElement;
            }
            if (thisSchemaElement == null)
                throw new NotImplementedException();
        }

        internal void Validate()
        {
            if (thisSchemaElement.ItemType == Element.ElementItemType.Monetary)
                ValidateMonetaryType();
        }

        private void ValidateMonetaryType()
        {
            if (thisUnitRef.MeasureQualifiedName != null)
            {
                string Uri = thisUnitRef.MeasureQualifiedName.NamespaceUri;
                if ((Uri.Length > 0) && (Uri.Equals("http://www.xbrl.org/2003/iso4217") == false))
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("WrongMeasureNamespaceForMonetaryFact");
                    MessageBuilder.AppendFormat(StringFormat, thisName, thisUnitRef.Id, thisUnitRef.MeasureQualifiedName.NamespaceUri);
                    throw new XbrlException(MessageBuilder.ToString());
                }
                thisUnitRef.SetCultureAndRegionInfoFromISO4217Code(thisUnitRef.MeasureQualifiedName.LocalName);
                if ((thisUnitRef.CultureInformation == null) && (thisUnitRef.RegionInformation == null))
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("UnsupportedISO4217CodeForUnitMeasure");
                    MessageBuilder.AppendFormat(StringFormat, thisName, thisUnitRef.Id, thisUnitRef.MeasureQualifiedName.LocalName);
                    throw new XbrlException(MessageBuilder.ToString());
                }
            }
        }
    }
}
