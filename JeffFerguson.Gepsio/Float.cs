using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class Float : AnySimpleType<float>
    {
        protected override float ConvertStringValue()
        {
            return float.Parse(ValueAsString);
        }
    }
}
