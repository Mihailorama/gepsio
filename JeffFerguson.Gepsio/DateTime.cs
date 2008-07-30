using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class DateTime : AnySimpleType<System.DateTime>
    {
        protected override System.DateTime ConvertStringValue()
        {
            System.DateTime Result;

            System.DateTime.TryParse(this.ValueAsString, out Result);
            return Result;
        }
    }
}
