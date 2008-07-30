using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class Double : AnySimpleType<double>
    {
        protected override double ConvertStringValue()
        {
            return double.Parse(ValueAsString);
        }
    }
}
