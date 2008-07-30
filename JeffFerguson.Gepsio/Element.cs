using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Element
    {
        public enum ElementSubstitutionGroup
        {
            Unknown,
            Item,
            Tuple
        }

        public enum ElementPeriodType
        {
            Unknown,
            Instant,
            Duration
        }

        private XmlNode thisElementNode;
        private string thisName;
        private string thisId;
        private XbrlSchema thisSchema;
        private ElementSubstitutionGroup thisSubstitutionGroup;
        private ElementPeriodType thisPeriodType;
        private AnyType thisItemType;

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

        public ElementSubstitutionGroup SubstitutionGroup
        {
            get
            {
                return thisSubstitutionGroup;
            }
        }

        public ElementPeriodType PeriodType
        {
            get
            {
                return thisPeriodType;
            }
        }

        public XbrlSchema Schema
        {
            get
            {
                return thisSchema;
            }
        }

        public AnyType Type
        {
            get
            {
                return thisItemType;
            }
        }

        internal Element(XbrlSchema Schema, XmlNode ElementNode)
        {
            thisSchema = Schema;
            thisElementNode = ElementNode;

            if (thisElementNode.Attributes["name"] != null)
                thisName = thisElementNode.Attributes["name"].Value;
            else
                thisName = string.Empty;

            if (thisElementNode.Attributes["type"] != null)
                SetItemType(thisElementNode.Attributes["type"].Value);

            if (thisElementNode.Attributes["substitutionGroup"] != null)
                SetSubstitutionGroup(thisElementNode.Attributes["substitutionGroup"].Value);
            else
                thisSubstitutionGroup = ElementSubstitutionGroup.Unknown;

            if (thisElementNode.Attributes["id"] != null)
                thisId = thisElementNode.Attributes["id"].Value;
            else
                thisId = string.Empty;

            SetPeriodType();
        }

        private void SetPeriodType()
        {
            string PeriodTypePrefix = thisSchema.UrisAndPrefixes.GetPrefixForUri("http://www.xbrl.org/2003/instance");
            string AttributeName;
            if (PeriodTypePrefix == null)
                AttributeName = "periodType";
            else
                AttributeName = PeriodTypePrefix + ":periodType";
            if (thisElementNode.Attributes[AttributeName] != null)
                SetPeriodType(thisElementNode.Attributes[AttributeName].Value);
            else
                thisPeriodType = ElementPeriodType.Unknown;
        }

        private void SetPeriodType(string PeriodType)
        {
            thisPeriodType = ElementPeriodType.Unknown;
            if (PeriodType == "instant")
                thisPeriodType = ElementPeriodType.Instant;
            else if (PeriodType == "duration")
                thisPeriodType = ElementPeriodType.Duration;
            else
            {

                // We can't identify the type, so throw an exception.

                string MessageFormat = AssemblyResources.GetName("InvalidElementPeriodType");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, thisSchema.Path, PeriodType, thisName);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        private void SetSubstitutionGroup(string SubstitutionGroupValue)
        {
            thisSubstitutionGroup = ElementSubstitutionGroup.Unknown;
            if (SubstitutionGroupValue.Contains("item") == true)
                thisSubstitutionGroup = ElementSubstitutionGroup.Item;
            else if (SubstitutionGroupValue.Contains("tuple") == true)
                thisSubstitutionGroup = ElementSubstitutionGroup.Tuple;
            else
            {

                // We can't identify the type, so throw an exception.

                string MessageFormat = AssemblyResources.GetName("InvalidElementSubstitutionGroup");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, thisSchema.Path, SubstitutionGroupValue, thisName);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

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

        private string GetLocalName(string FullName)
        {
            int ColonIndex = FullName.IndexOf(':');
            if (ColonIndex == -1)
                return FullName;
            return FullName.Substring(ColonIndex + 1);
        }
    }
}
