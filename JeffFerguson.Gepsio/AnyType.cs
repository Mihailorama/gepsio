using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public abstract class AnyType
    {
        private string thisValue;

        public virtual string ValueAsString
        {
            get
            {
                return thisValue;
            }
            set
            {
                thisValue = value;
            }
        }

        public abstract bool NumericType
        {
            get;
        }

        internal AnyType()
        {
        }

        internal virtual void ValidateFact(Item FactToValidate)
        {
        }

        public static AnyType CreateType(string TypeName)
        {
            return AnyType.CreateType(TypeName, null);
        }

        internal abstract decimal GetValueAfterApplyingPrecisionTruncation(int PrecisionValue);

        internal abstract decimal GetValueAfterApplyingDecimalsTruncation(int DecimalsValue);

        public static AnyType CreateType(string TypeName, XmlNode SchemaRootNode)
        {
            AnyType TypeToReturn;

            switch (TypeName)
            {
                case "token":
                    TypeToReturn = new Token(SchemaRootNode);
                    break;
                case "string":
                    TypeToReturn = new String(SchemaRootNode);
                    break;
                case "xbrli:decimalItemType":
                    TypeToReturn = new DecimalItemType();
                    break;
                case "xbrli:monetaryItemType":
                    TypeToReturn = new MonetaryItemType();
                    break;
                case "xbrli:pureItemType":
                    TypeToReturn = new PureItemType();
                    break;
                case "xbrli:sharesItemType":
                    TypeToReturn = new SharesItemType();
                    break;
                case "xbrli:tokenItemType":
                    TypeToReturn = new TokenItemType();
                    break;
                case "xbrli:stringItemType":
                    TypeToReturn = new StringItemType();
                    break;
                default:
                    TypeToReturn = null;
                    break;
            }
            return TypeToReturn;
        }
    }
}
