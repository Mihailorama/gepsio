using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    internal class TokenItemType : ComplexType
    {
        internal TokenItemType()
            : base("tokenItemType", new Token(null), new NonNumericItemAttributes())
        {
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            base.ValidateFact(FactToValidate);
        }
    }
}
