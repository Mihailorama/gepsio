using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class DecimalItemType : ComplexType
    {
        internal DecimalItemType()
            : base("decimalItemType", new Decimal(null), new NumericItemAttributes())
        {
        }
    }
}
