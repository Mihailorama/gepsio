using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class GregorianYear : AnySimpleType<System.DateTime>
    {
        protected override System.DateTime ConvertStringValue()
        {
            System.DateTime Result;

            System.DateTime.TryParse(this.ValueAsString, out Result);
            return Result;
        }
    }
}
