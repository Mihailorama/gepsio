using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class Boolean : AnySimpleType<bool>
    {
        protected override bool ConvertStringValue()
        {
            bool Result;

            bool.TryParse(this.ValueAsString, out Result);
            return Result;
        }
    }
}
