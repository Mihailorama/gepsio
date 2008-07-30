using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Decimal : AnySimpleType<double>
    {
        internal Decimal(XmlNode StringRootNode) : base(StringRootNode)
        {
        }

        protected override double ConvertStringValue()
        {
            return double.Parse(ValueAsString);
        }
    }
}
