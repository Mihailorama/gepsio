using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class MonetaryItemType : ComplexType
    {
        internal MonetaryItemType()
            : base("monetaryItemType", new Monetary(null), new NumericItemAttributes())
        {
        }
    }
}
