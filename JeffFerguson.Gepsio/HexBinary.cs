﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class HexBinary : AnySimpleType<int>
    {
        protected override int ConvertStringValue()
        {
            return int.Parse(ValueAsString);
        }
    }
}