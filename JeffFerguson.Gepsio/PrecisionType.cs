﻿using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio
{
    internal class PrecisionType : NonNegativeInteger
    {
        internal PrecisionType(INode StringRootNode) : base(StringRootNode)
        {
        }
    }
}
