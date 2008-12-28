using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    internal class NumericItemAttributes : EssentialNumericItemAttributes
    {
        internal NumericItemAttributes()
            : base()
        {
            AddAttribute(new Attribute("precision", typeof(PrecisionType), false));
            AddAttribute(new Attribute("decimals", typeof(DecimalsType), false));
        }
    }
}
