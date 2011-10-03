using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio
{
    public class Item : Fact
    {
        private string thisContextRefName;
        private string thisUnitRefName;
        private string thisPrecisionAttributeValue;
        private int thisPrecision;
        private int thisDecimals;
        private string thisDecimalsAttributeValue;
        private string thisValue;
        private Context thisContextRef;
        private Unit thisUnitRef;
        private Element thisSchemaElement;
        private string thisId;
        private bool thisNilSpecified;
        private XmlSchemaType thisItemType;
        private XbrlSchema thisSchema;
        private bool thisInfinitePrecision;
        private bool thisInfiniteDecimals;
        private bool thisPrecisionInferred;
        private bool thisRoundedValueCalculated;
        private double thisRoundedValue;

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
        public bool InfinitePrecision
        {
            get
            {
                return thisInfinitePrecision;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public bool PrecisionInferred
        {
            get
            {
                return thisPrecisionInferred;
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
        public bool InfiniteDecimals
        {
            get
            {
                return thisInfiniteDecimals;
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
        public XmlSchemaType Type
        {
            get
            {
                return thisItemType;
            }
        }

        /// <summary>
        /// Returns the rounded value of the fact's actual value. The rounded value is calculated from the precision
        /// (or imferred precision) of the fact's actual value.
        /// </summary>
        public double RoundedValue
        {
            get
            {
                if (thisRoundedValueCalculated == false)
                {
                    thisRoundedValue = GetRoundedValue();
                    thisRoundedValueCalculated = true;
                }
                return thisRoundedValue;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal Item(XbrlFragment ParentFragment, XmlNode ItemNode) : base(ParentFragment, ItemNode)
        {
            thisContextRefName = XmlUtilities.GetAttributeValue(thisFactNode, "contextRef");
            thisUnitRefName = XmlUtilities.GetAttributeValue(thisFactNode, "unitRef");
            thisRoundedValueCalculated = false;

            thisDecimalsAttributeValue = XmlUtilities.GetAttributeValue(thisFactNode, "decimals");
            if (thisDecimalsAttributeValue.Length > 0)
            {
                if (thisDecimalsAttributeValue.Equals("INF") == true)
                {
                    thisInfiniteDecimals = true;
                    thisDecimals = 0;
                }
                else
                {
                    thisInfiniteDecimals = false;
                    thisDecimals = Convert.ToInt32(thisDecimalsAttributeValue);
                }
            }

            thisPrecisionAttributeValue = XmlUtilities.GetAttributeValue(thisFactNode, "precision");
            if (thisPrecisionAttributeValue.Length > 0)
            {
                thisPrecisionInferred = false;
                if (thisPrecisionAttributeValue.Equals("INF") == true)
                {
                    thisInfinitePrecision = true;
                    thisPrecision = 0;
                }
                else
                {
                    thisInfinitePrecision = false;
                    thisPrecision = Convert.ToInt32(thisPrecisionAttributeValue);
                }
            }

            thisId = XmlUtilities.GetAttributeValue(thisFactNode, "id");
            thisNilSpecified = false;
            string NilValue = XmlUtilities.GetAttributeValue(thisFactNode, "http://www.w3.org/2001/XMLSchema-instance", "nil");
            if (NilValue.Equals("true", StringComparison.CurrentCultureIgnoreCase) == true)
                thisNilSpecified = true;
            GetSchemaElementFromSchema();
            thisValue = thisFactNode.InnerText;
            if (SchemaElement.SubstitutionGroup == Element.ElementSubstitutionGroup.Item)
                SetItemType(SchemaElement.TypeName);

            if (thisPrecisionAttributeValue.Length == 0)
                InferPrecision();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void InferPrecision()
        {
            thisPrecisionInferred = true;
            int CalculationPart1Value = GetNumberOfDigitsToLeftOfDecimalPoint();
            if (CalculationPart1Value == 0)
                CalculationPart1Value = GetNegativeNumberOfLeadingZerosToRightOfDecimalPoint();
            int CalculationPart2Value = GetExponentValue();
            int CalculationPart3Value = thisDecimals;
            thisPrecision = CalculationPart1Value + CalculationPart2Value + CalculationPart3Value;
            if (thisPrecision < 0)
                thisPrecision = 0;
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
            {
                if (thisParentFragment.Schemas.Count == 0)
                {
                    string MessageFormat = AssemblyResources.GetName("NoSchemasForFragment");
                    StringBuilder MessageFormatBuilder = new StringBuilder();
                    MessageFormatBuilder.AppendFormat(MessageFormat);
                    throw new XbrlException(MessageFormatBuilder.ToString());
                }
                thisSchema = thisParentFragment.Schemas[0];
            }
            foreach (Element CurrentElement in thisSchema.Elements)
            {
                if (CurrentElement.Name == this.Name)
                    thisSchemaElement = CurrentElement;
            }
            if (thisSchemaElement == null)
            {
                string MessageFormat = AssemblyResources.GetName("CannotFindFactElementInSchema");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, this.Name, thisSchema.Path);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetItemType(XmlQualifiedName ItemTypeValue)
        {
            thisItemType = thisSchema.GetXmlSchemaType(ItemTypeValue);
            if (thisItemType == null)
            {
                string MessageFormat = AssemblyResources.GetName("InvalidElementItemType");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, thisSchema.Path, ItemTypeValue, this.Name);
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

        /// <summary>
        /// Determines whether or not the item type for this fact is a monetary type.
        /// </summary>
        /// <returns>
        /// True if the type for this fact is a monetary type and false otherwise.
        /// </returns>
        internal bool IsMonetary()
        {
            return TypeNameContains("monetary");
        }

        /// <summary>
        /// Determines whether or not the item type for this fact is a pure type.
        /// </summary>
        /// <returns>
        /// True if the type for this fact is a pure type and false otherwise.
        /// </returns>
        internal bool IsPure()
        {
            return TypeNameContains("pure");
        }

        /// <summary>
        /// Determines whether or not the item type for this fact is a shares type.
        /// </summary>
        /// <returns>
        /// True if the type for this fact is a shares type and false otherwise.
        /// </returns>
        internal bool IsShares()
        {
            return TypeNameContains("shares");
        }

        /// <summary>
        /// Determines whether or not the item type for this fact is a decimal type.
        /// </summary>
        /// <returns>
        /// True if the type for this fact is a decimal type and false otherwise.
        /// </returns>
        internal bool IsDecimal()
        {
            return TypeNameContains("decimal");
        }

        /// <summary>
        /// Determines whether or not the fact's item type is of the given type.
        /// </summary>
        /// <returns>
        /// True if the type is of the given type and false otherwise.
        /// </returns>
        private bool TypeNameContains(string TypeName)
        {
            try
            {
                return TypeNameContains(TypeName, thisItemType);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether or not the supplied item type is of the given type.
        /// </summary>
        /// <returns>
        /// True if the type is of the given type and false otherwise.
        /// </returns>
        private bool TypeNameContains(string TypeName, XmlSchemaType CurrentType)
        {
            if (CurrentType.Name.Contains(TypeName) == true)
                return true;
            if (CurrentType is XmlSchemaComplexType)
            {
                XmlSchemaComplexType CurrentComplexType = CurrentType as XmlSchemaComplexType;
                if (CurrentComplexType.DerivedBy == XmlSchemaDerivationMethod.Restriction)
                    return TypeNameContains(TypeName, CurrentComplexType.BaseXmlSchemaType);
            }
            return false;
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
            if (IsMonetary())
                ValidateMonetaryType();
            else if (IsPure())
                ValidatePureType();
            else if (IsShares())
                ValidateSharesType();
            else if (IsDecimal())
                ValidateDecimalType();
        }

        private void ValidateMonetaryType()
        {
            Unit UnitReference = UnitRef;
            if (UnitReference == null)
                return;
            if (UnitReference.MeasureQualifiedNames[0] == null)
                return;

            string Uri = UnitReference.MeasureQualifiedNames[0].NamespaceUri;
            if (Uri == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("WrongMeasureNamespaceForMonetaryFact");
                MessageBuilder.AppendFormat(StringFormat, Name, UnitReference.Id, "unspecified");
                throw new XbrlException(MessageBuilder.ToString());
            }

            if ((Uri.Length > 0) && (Uri.Equals("http://www.xbrl.org/2003/iso4217") == false))
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("WrongMeasureNamespaceForMonetaryFact");
                MessageBuilder.AppendFormat(StringFormat, Name, UnitReference.Id, UnitReference.MeasureQualifiedNames[0].NamespaceUri);
                throw new XbrlException(MessageBuilder.ToString());
            }
            UnitReference.SetCultureAndRegionInfoFromISO4217Code(UnitReference.MeasureQualifiedNames[0].LocalName);
            if ((UnitReference.CultureInformation == null) && (UnitReference.RegionInformation == null))
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("UnsupportedISO4217CodeForUnitMeasure");
                MessageBuilder.AppendFormat(StringFormat, Name, UnitReference.Id, UnitReference.MeasureQualifiedNames[0].LocalName);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        /// <summary>
        /// Validate pure item types.
        /// </summary>
        private void ValidatePureType()
        {
            string UnitMeasureLocalName = string.Empty;
            Unit UnitReference = UnitRef;
            bool PureMeasureFound = true;
            if (UnitReference.MeasureQualifiedNames.Count != 1)
                PureMeasureFound = false;
            if (PureMeasureFound == true)
            {
                UnitMeasureLocalName = UnitReference.MeasureQualifiedNames[0].LocalName;
                PureMeasureFound = UnitMeasureLocalName.Equals("pure");
            }
            if (PureMeasureFound == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("PureItemTypeUnitLocalNameNotPure");
                MessageBuilder.AppendFormat(StringFormat, Name, UnitReference.Id, UnitMeasureLocalName);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        /// <summary>
        /// Validate shares item types.
        /// </summary>
        private void ValidateSharesType()
        {
            bool SharesMeasureFound = true;
            string UnitMeasureLocalName = string.Empty;
            Unit UnitReference = UnitRef;
            if (UnitReference.MeasureQualifiedNames.Count != 1)
                SharesMeasureFound = false;
            if (SharesMeasureFound == true)
            {
                UnitMeasureLocalName = UnitReference.MeasureQualifiedNames[0].LocalName;
                SharesMeasureFound = UnitMeasureLocalName.Equals("shares");
            }
            if (SharesMeasureFound == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SharesItemTypeUnitLocalNameNotShares");
                MessageBuilder.AppendFormat(StringFormat, Name, UnitReference.Id, UnitMeasureLocalName);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        /// <summary>
        /// Validate decimal item types.
        /// </summary>
        private void ValidateDecimalType()
        {
            if (NilSpecified == true)
                ValidateNilDecimalType();
            else
                ValidateNonNilDecimalType();
        }

        private void ValidateNonNilDecimalType()
        {
            if ((PrecisionSpecified == false) && (DecimalsSpecified == false))
            {
                string MessageFormat = AssemblyResources.GetName("NumericFactWithoutSpecifiedPrecisionOrDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, Name, Id);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
            if ((PrecisionSpecified == true) && (DecimalsSpecified == true))
            {
                string MessageFormat = AssemblyResources.GetName("NumericFactWithSpecifiedPrecisionAndDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, Name, Id);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        private void ValidateNilDecimalType()
        {
            if ((PrecisionSpecified == true) || (DecimalsSpecified == true))
            {
                string MessageFormat = AssemblyResources.GetName("NilNumericFactWithSpecifiedPrecisionOrDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, Name, Id);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Context Equal (c-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool ContextEquals(Item OtherFact)
        {
            if (Object.ReferenceEquals(this.ContextRef, OtherFact.ContextRef) == true)
                return true;
            return this.ContextRef.StructureEquals(OtherFact.ContextRef);
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Parent Equal (p-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool ParentEquals(Item OtherFact)
        {
            if (thisFactNode == null)
                return false;
            return thisFactNode.ParentEquals(OtherFact.thisFactNode);
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Unit Equal (u-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool UnitEquals(Item OtherFact)
        {
            if (OtherFact == null)
                return false;
            if (this.UnitRef == null)
                return true;
            if (OtherFact.UnitRef == null)
                return true;
            return this.UnitRef.StructureEquals(OtherFact.UnitRef);
        }

        /// <summary>
        /// Calculates the number of digits to the left of the decimal point. Leading zeros are not counted.
        /// </summary>
        /// <returns>
        /// The number of digits to the left of the decimal point. Leading zeros are not counted.
        /// </returns>
        private int GetNumberOfDigitsToLeftOfDecimalPoint()
        {
            if (thisValue == null)
                return 0;
            string[] ParsedValue = ParseValueIntoComponentParts();
            string WithoutLeadingZeros = ParsedValue[0].TrimStart(new char[] { '0' });
            return WithoutLeadingZeros.Length;
        }

        /// <summary>
        /// Calculates the negative number of leading zeros to the right of the decimal point.
        /// </summary>
        /// <returns>
        /// The negative number of leading zeros to the right of the decimal point.
        /// </returns>
        private int GetNegativeNumberOfLeadingZerosToRightOfDecimalPoint()
        {
            if (thisValue == null)
                return 0;
            string[] ParsedValue = ParseValueIntoComponentParts();
            string ValueToTheRightOfTheDecimal = ParsedValue[1];
            if (string.IsNullOrEmpty(ValueToTheRightOfTheDecimal))
                return 0;
            int NumberOfLeadingZeros = 0;
            int Index = 0;
            while (Index < ValueToTheRightOfTheDecimal.Length)
            {
                if (ValueToTheRightOfTheDecimal[Index] == '0')
                {
                    NumberOfLeadingZeros++;
                    Index++;
                }
                else
                {
                    Index = ValueToTheRightOfTheDecimal.Length;
                    break;
                }
            }
            return -NumberOfLeadingZeros;
        }

        /// <summary>
        /// Calculates the value of the exponent in the lexical representation of the fact value.
        /// </summary>
        /// <returns>
        /// The value of the exponent in the lexical representation of the fact value.
        /// </returns>
        private int GetExponentValue()
        {
            if (thisValue == null)
                return 0;
            string[] ParsedValue = ParseValueIntoComponentParts();
            if (string.IsNullOrEmpty(ParsedValue[2]))
                return 0;
            int ExponentValue;
            bool Success = int.TryParse(ParsedValue[2], out ExponentValue);
            if (Success == true)
                return ExponentValue;
            return 0;
        }

        /// <summary>
        /// <para>
        /// Parses the fact value into three main parts:
        /// </para>
        /// <list>
        /// <item>
        /// Values to the left of the decimal point
        /// </item>
        /// <item>
        /// Values to the right of the decimal point
        /// </item>
        /// <item>
        /// Exponent value
        /// </item>
        /// </list>
        /// </summary>
        /// <para>
        /// Some of these values may be empty if the original value did not carry all of these components.
        /// </para>
        /// <returns>A string array of length 3. Item 0 contains the value before the decimal point,
        /// item 1 contains the value after the decimal point, and item 2 contains the exponent value. If any
        /// of these elements are not available in the original value, then their individual value within the
        /// array will be empty.</returns>
        private string[] ParseValueIntoComponentParts()
        {
            return ParseValueIntoComponentParts(thisValue);
        }

        /// <summary>
        /// <para>
        /// Parses a string representation of a fact value into three main parts:
        /// </para>
        /// <list>
        /// <item>
        /// Values to the left of the decimal point
        /// </item>
        /// <item>
        /// Values to the right of the decimal point
        /// </item>
        /// <item>
        /// Exponent value
        /// </item>
        /// </list>
        /// </summary>
        /// <para>
        /// Some of these values may be empty if the original value did not carry all of these components.
        /// </para>
        /// <returns>A string array of length 3. Item 0 contains the value before the decimal point,
        /// item 1 contains the value after the decimal point, and item 2 contains the exponent value. If any
        /// of these elements are not available in the original value, then their individual value within the
        /// array will be empty.</returns>
        private string[] ParseValueIntoComponentParts(string OriginalValue)
        {
            string[] ArrayToReturn = new string[3];

            string[] StringsAfterExponentSplit = OriginalValue.Split(new char[] { 'e', 'E' });
            if (StringsAfterExponentSplit.Length == 2)
                ArrayToReturn[2] = StringsAfterExponentSplit[1];
            string NumericValue = StringsAfterExponentSplit[0];
            string[] StringsAfterDecimalSplit = NumericValue.Split(new char[] { '.' });
            ArrayToReturn[0] = StringsAfterDecimalSplit[0];
            if (StringsAfterDecimalSplit.Length == 2)
                ArrayToReturn[1] = StringsAfterDecimalSplit[1];
            return ArrayToReturn;
        }

        private double GetRoundedValue()
        {
            double RoundedValue = Convert.ToDouble(thisValue);
            return Round(RoundedValue);
        }

        /// <summary>
        ///  Round a given value using the Precision already available in the fact.
        /// </summary>
        /// <param name="OriginalValue"></param>
        /// <returns></returns>
        public double Round(double OriginalValue)
        {
            double RoundedValue = OriginalValue;
            if (InfinitePrecision == false)
            {
                // Break the original value into three parts: (1) values to the left of the decimal, (2) values to the right of the decimal,
                // and (3) the exponent value. Remember that one or more of these, particularly parts (2) and (3), may be empty or null.
                string OriginalValueAsString = OriginalValue.ToString();
                string[] ComponentParts = ParseValueIntoComponentParts(OriginalValueAsString);
                ComponentParts[0] = ComponentParts[0].TrimStart(new char[] { '0' });
                if (string.IsNullOrEmpty(ComponentParts[1]) == false)
                    ComponentParts[1] = ComponentParts[1].TrimEnd(new char[] { '0' });
                if (Precision > ComponentParts[0].Length)
                {
                    // In this case, the Precision value is greater than the length of the portion of the value to the left of the decimal.
                    // An example of this may be a precision of 5 and a value of "123.456". The length of the portion of the value to the left
                    // of the decimal ("123") is 3 and the precision is 5. In this situation, we will need to round to a number of places
                    // to the right of the decimal. Since the precision is 5, and since three of those five will be used for the left of the decimal,
                    // then we are left with two places to round to the right of the decimal.
                    RoundedValue = Math.Round(RoundedValue, Precision - ComponentParts[0].Length);
                }
                else if (Precision == ComponentParts[0].Length)
                {
                    // In this case, the Precision value is equal to the length of the portion of the value to the left of the decimal. In this case,
                    // we'll simply round to the nearest integer.
                    RoundedValue = Math.Round(RoundedValue);
                }
                else
                {
                    // In this case, the Precision value is less than the length of the portion of the value to the left of the decimal. We need, therefore,
                    // to round a whole number -- that part of the number stored as the first component part.
                    double PowerOfTen = Math.Pow(10.0, (double)(ComponentParts[0].Length - Precision));
                    RoundedValue = RoundedValue / PowerOfTen;
                    RoundedValue = Math.Round(RoundedValue);
                    RoundedValue = RoundedValue * PowerOfTen;
                }
            }
            return RoundedValue;
        }
    }
}
