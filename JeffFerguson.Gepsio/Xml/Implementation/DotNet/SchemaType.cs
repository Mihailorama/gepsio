using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio.Xml.Implementation.DotNet
{
    internal class SchemaType : ISchemaType
    {
        private XmlSchemaType schemaType;
        private IQualifiedName thisQualifiedName;
        private ISchemaType thisBaseSchemaType;

        public IQualifiedName QualifiedName
        {
            get
            {
                if (thisQualifiedName == null)
                    thisQualifiedName = new QualifiedName(schemaType.QualifiedName);
                return thisQualifiedName;
            }
        }

        public string Name
        {
            get
            {
                return schemaType.Name;
            }
        }

        public bool IsNumeric
        {
            get
            {
                switch (schemaType.TypeCode)
                {
                    case XmlTypeCode.Decimal:
                    case XmlTypeCode.Double:
                    case XmlTypeCode.Float:
                    case XmlTypeCode.Int:
                    case XmlTypeCode.Integer:
                    case XmlTypeCode.Long:
                    case XmlTypeCode.NegativeInteger:
                    case XmlTypeCode.NonNegativeInteger:
                    case XmlTypeCode.NonPositiveInteger:
                    case XmlTypeCode.PositiveInteger:
                    case XmlTypeCode.Short:
                    case XmlTypeCode.UnsignedInt:
                    case XmlTypeCode.UnsignedLong:
                    case XmlTypeCode.UnsignedShort:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsComplex
        {
            get
            {
                if (schemaType is XmlSchemaComplexType)
                    return true;
                return false;
            }
        }

        public bool DerivedByRestriction
        {
            get
            {
                if (IsComplex == false)
                    return false;
                XmlSchemaComplexType CurrentComplexType = schemaType as XmlSchemaComplexType;
                if (CurrentComplexType.DerivedBy == XmlSchemaDerivationMethod.Restriction)
                    return true;
                return false;
            }
        }

        public ISchemaType BaseSchemaType
        {
            get
            {
                if (DerivedByRestriction == false)
                    return null;
                if (thisBaseSchemaType == null)
                {
                    XmlSchemaComplexType CurrentComplexType = schemaType as XmlSchemaComplexType;
                    thisBaseSchemaType = new SchemaType(CurrentComplexType.BaseXmlSchemaType);
                }
                return thisBaseSchemaType;
            }
        }

        internal SchemaType(XmlSchemaType type)
        {
            schemaType = type;
            thisQualifiedName = null;
            thisBaseSchemaType = null;
        }
    }
}
