using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Element
    {
        public enum ElementItemType
        {
            // derived from section 5.1.1.3 of the XBRL spec

            Unknown,
            Decimal,
            Float,
            Double,
            Integer,
            NonPositiveInteger,
            NegativeInteger,
            Long,
            Int,
            Short,
            Byte,
            NonNegativeInteger,
            UnsignedLong,
            UnsignedInt,
            UnsignedShort,
            UnsignedByte,
            PositiveInteger,
            Monetary,
            Shares,
            Pure,
            Fraction,
            String,
            Boolean,
            HexBinary,
            Base64Binary,
            AnyUri,
            Qname,
            Duration,
            DateTime,
            Time,
            Date,
            YearMonth,
            Year,
            MonthDay,
            Day,
            Month,
            NormalizedString,
            Token,
            Language,
            Name,
            NCName,
            SimpleType,
            ComplexType
        }

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
        private ElementItemType thisItemType;
        private XbrlSchema thisSchema;
        private ElementSubstitutionGroup thisSubstitutionGroup;
        private ElementPeriodType thisPeriodType;
        private SimpleType thisSimpleTypeDeclaration;
        private ComplexType thisComplexTypeDeclaration;

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

        public ElementItemType ItemType
        {
            get
            {
                return thisItemType;
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

        public SimpleType SimpleTypeDeclaration
        {
            get
            {
                return thisSimpleTypeDeclaration;
            }
        }

        public ComplexType ComplexTypeDeclaration
        {
            get
            {
                return thisComplexTypeDeclaration;
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
            else
                thisItemType = ElementItemType.Unknown;

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
            thisSimpleTypeDeclaration = null;
            thisItemType = ElementItemType.Unknown;
            if (ItemTypeValue.Contains("decimalItemType") == true)
                thisItemType = ElementItemType.Decimal;
            else if (ItemTypeValue.Contains("floatItemType") == true)
                thisItemType = ElementItemType.Float;
            else if (ItemTypeValue.Contains("doubleItemType") == true)
                thisItemType = ElementItemType.Double;
            else if (ItemTypeValue.Contains("integerItemType") == true)
                thisItemType = ElementItemType.Integer;
            else if (ItemTypeValue.Contains("nonPositiveIntegerItemType") == true)
                thisItemType = ElementItemType.NonPositiveInteger;
            else if (ItemTypeValue.Contains("negativeIntegerItemType") == true)
                thisItemType = ElementItemType.NegativeInteger;
            else if (ItemTypeValue.Contains("longItemType") == true)
                thisItemType = ElementItemType.Long;
            else if (ItemTypeValue.Contains("intItemType") == true)
                thisItemType = ElementItemType.Int;
            else if (ItemTypeValue.Contains("shortItemType") == true)
                thisItemType = ElementItemType.Short;
            else if (ItemTypeValue.Contains("byteItemType") == true)
                thisItemType = ElementItemType.Byte;
            else if (ItemTypeValue.Contains("nonNegativeIntegerItemType") == true)
                thisItemType = ElementItemType.NonNegativeInteger;
            else if (ItemTypeValue.Contains("unsignedLongItemType") == true)
                thisItemType = ElementItemType.UnsignedLong;
            else if (ItemTypeValue.Contains("unsignedIntItemType") == true)
                thisItemType = ElementItemType.UnsignedInt;
            else if (ItemTypeValue.Contains("unsignedShortItemType") == true)
                thisItemType = ElementItemType.UnsignedShort;
            else if (ItemTypeValue.Contains("unsignedByteItemType") == true)
                thisItemType = ElementItemType.UnsignedByte;
            else if (ItemTypeValue.Contains("positiveIntegerItemType") == true)
                thisItemType = ElementItemType.PositiveInteger;
            else if (ItemTypeValue.Contains("monetaryItemType") == true)
                thisItemType = ElementItemType.Monetary;
            else if (ItemTypeValue.Contains("sharesItemType") == true)
                thisItemType = ElementItemType.Shares;
            else if (ItemTypeValue.Contains("pureItemType") == true)
                thisItemType = ElementItemType.Pure;
            else if (ItemTypeValue.Contains("fractionItemType") == true)
                thisItemType = ElementItemType.Fraction;
            else if (ItemTypeValue.Contains("stringItemType") == true)
                thisItemType = ElementItemType.String;
            else if (ItemTypeValue.Contains("booleanItemType") == true)
                thisItemType = ElementItemType.Boolean;
            else if (ItemTypeValue.Contains("hexBinaryItemType") == true)
                thisItemType = ElementItemType.HexBinary;
            else if (ItemTypeValue.Contains("base64BinaryItemType") == true)
                thisItemType = ElementItemType.Base64Binary;
            else if (ItemTypeValue.Contains("anyURIItemType") == true)
                thisItemType = ElementItemType.AnyUri;
            else if (ItemTypeValue.Contains("QNameItemType") == true)
                thisItemType = ElementItemType.Qname;
            else if (ItemTypeValue.Contains("durationItemType") == true)
                thisItemType = ElementItemType.Duration;
            else if (ItemTypeValue.Contains("dateTimeItemType") == true)
                thisItemType = ElementItemType.DateTime;
            else if (ItemTypeValue.Contains("timeItemType") == true)
                thisItemType = ElementItemType.Time;
            else if (ItemTypeValue.Contains("dateItemType") == true)
                thisItemType = ElementItemType.Date;
            else if (ItemTypeValue.Contains("gYearMonthItemType") == true)
                thisItemType = ElementItemType.YearMonth;
            else if (ItemTypeValue.Contains("gYearItemType") == true)
                thisItemType = ElementItemType.Year;
            else if (ItemTypeValue.Contains("gMonthDayItemType") == true)
                thisItemType = ElementItemType.MonthDay;
            else if (ItemTypeValue.Contains("gDayItemType") == true)
                thisItemType = ElementItemType.Day;
            else if (ItemTypeValue.Contains("gMonthItemType") == true)
                thisItemType = ElementItemType.Month;
            else if (ItemTypeValue.Contains("normalizedStringItemType") == true)
                thisItemType = ElementItemType.NormalizedString;
            else if (ItemTypeValue.Contains("tokenItemType") == true)
                thisItemType = ElementItemType.Token;
            else if (ItemTypeValue.Contains("languageItemType") == true)
                thisItemType = ElementItemType.Language;
            else if (ItemTypeValue.Contains("NameItemType") == true)
                thisItemType = ElementItemType.Name;
            else if (ItemTypeValue.Contains("NCNameItemType") == true)
                thisItemType = ElementItemType.NCName;
            else if (IsSimpleType(ItemTypeValue) == true)
            {
                SetSimpleType(ItemTypeValue);
                thisItemType = ElementItemType.SimpleType;
            }
            else if (IsComplexType(ItemTypeValue) == true)
            {
                SetComplexType(ItemTypeValue);
                thisItemType = ElementItemType.ComplexType;
            }
            else
            {

                // We can't identify the type, so throw an exception.

                string MessageFormat = AssemblyResources.GetName("InvalidElementItemType");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, thisSchema.Path, ItemTypeValue, thisName);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        private void SetSimpleType(string ItemTypeValue)
        {
            string LocalName = GetLocalName(ItemTypeValue);
            foreach (SimpleType CurrentSimpleType in thisSchema.SimpleTypes)
            {
                if (CurrentSimpleType.Name.Equals(LocalName) == true)
                {
                    thisSimpleTypeDeclaration = CurrentSimpleType;
                    break;
                }
            }
        }

        private void SetComplexType(string ItemTypeValue)
        {
            string LocalName = GetLocalName(ItemTypeValue);
            foreach (ComplexType CurrentComplexType in thisSchema.ComplexTypes)
            {
                if (CurrentComplexType.Name.Equals(LocalName) == true)
                {
                    thisComplexTypeDeclaration = CurrentComplexType;
                    break;
                }
            }
        }

        private bool IsSimpleType(string ItemTypeValue)
        {
            string LocalName = GetLocalName(ItemTypeValue);
            foreach (SimpleType CurrentSimpleType in thisSchema.SimpleTypes)
            {
                if (CurrentSimpleType.Name.Equals(LocalName) == true)
                    return true;
            }
            return false;
        }

        private bool IsComplexType(string ItemTypeValue)
        {
            string LocalName = GetLocalName(ItemTypeValue);
            foreach (ComplexType CurrentComplexType in thisSchema.ComplexTypes)
            {
                if (CurrentComplexType.Name.Equals(LocalName) == true)
                    return true;
            }
            return false;
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
