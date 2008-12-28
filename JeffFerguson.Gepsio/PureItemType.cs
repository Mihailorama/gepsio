using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    internal class PureItemType : ComplexType
    {
        internal PureItemType()
            : base("pureItemType", new Pure(null), new NumericItemAttributes())
        {
        }

        internal override void ValidateFact(Fact FactToValidate)
        {
            Unit UnitReference = FactToValidate.UnitRef;
            string UnitMeasureLocalName = UnitReference.MeasureQualifiedName.LocalName;
            if (UnitMeasureLocalName.Equals("pure") == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("PureItemTypeUnitLocalNameNotPure");
                MessageBuilder.AppendFormat(StringFormat, FactToValidate.Name, UnitReference.Id, UnitMeasureLocalName);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }
    }
}
