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
        internal Fact(XbrlFragment ParentFragment, XmlNode FactNode)
        {
            thisParentFragment = ParentFragment;
            thisFactNode = FactNode;
            thisName = thisFactNode.LocalName;
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
            {
                SetItemType(SchemaElement.TypeName);
                thisItemType.ValueAsString = thisValue;
            }

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
            if(Success == true)
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
