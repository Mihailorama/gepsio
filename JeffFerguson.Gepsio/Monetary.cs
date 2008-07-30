using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Monetary : Decimal
    {
        internal Monetary(XmlNode StringRootNode) : base(StringRootNode)
        {
        }

        internal override void ValidateFact(Fact FactToValidate)
        {
            Unit UnitReference = FactToValidate.UnitRef;
            if (UnitReference.MeasureQualifiedName != null)
            {
                string Uri = UnitReference.MeasureQualifiedName.NamespaceUri;
                if ((Uri.Length > 0) && (Uri.Equals("http://www.xbrl.org/2003/iso4217") == false))
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("WrongMeasureNamespaceForMonetaryFact");
                    MessageBuilder.AppendFormat(StringFormat, FactToValidate.Name, UnitReference.Id, UnitReference.MeasureQualifiedName.NamespaceUri);
                    throw new XbrlException(MessageBuilder.ToString());
                }
                UnitReference.SetCultureAndRegionInfoFromISO4217Code(UnitReference.MeasureQualifiedName.LocalName);
                if ((UnitReference.CultureInformation == null) && (UnitReference.RegionInformation == null))
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("UnsupportedISO4217CodeForUnitMeasure");
                    MessageBuilder.AppendFormat(StringFormat, FactToValidate.Name, UnitReference.Id, UnitReference.MeasureQualifiedName.LocalName);
                    throw new XbrlException(MessageBuilder.ToString());
                }
            }
        }
    }
}
