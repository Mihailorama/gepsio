using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public abstract class AnyType
    {
        public abstract string ValueAsString
        {
            get;
            set;
        }

        internal AnyType()
        {
        }

        internal virtual void ValidateFact(Fact FactToValidate)
        {
        }

        public static AnyType CreateType(string TypeName)
        {
            return AnyType.CreateType(TypeName, null);
        }

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
                default:
                    TypeToReturn = null;
                    break;
            }
            return TypeToReturn;
        }
    }
}
