using System.Xml;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class Decimal : AnySimpleType
    {
        internal Decimal(XmlNode StringRootNode) : base(StringRootNode)
        {
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            base.ValidateFact(FactToValidate);

            if (FactToValidate.NilSpecified == true)
                ValidateNilFact(FactToValidate);
            else
                ValidateNonNilFact(FactToValidate);
        }

        private void ValidateNilFact(Item FactToValidate)
        {
            if ((FactToValidate.PrecisionSpecified == true) || (FactToValidate.DecimalsSpecified == true))
            {
                string MessageFormat = AssemblyResources.GetName("NilNumericFactWithSpecifiedPrecisionOrDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, FactToValidate.Name, FactToValidate.Id);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        private void ValidateNonNilFact(Item FactToValidate)
        {
            if ((FactToValidate.PrecisionSpecified == false) && (FactToValidate.DecimalsSpecified == false))
            {
                string MessageFormat = AssemblyResources.GetName("NumericFactWithoutSpecifiedPrecisionOrDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, FactToValidate.Name, FactToValidate.Id);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
            if ((FactToValidate.PrecisionSpecified == true) && (FactToValidate.DecimalsSpecified == true))
            {
                string MessageFormat = AssemblyResources.GetName("NumericFactWithSpecifiedPrecisionAndDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, FactToValidate.Name, FactToValidate.Id);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }
    }
}
