using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class NonNegativeInteger : Integer
    {
        internal NonNegativeInteger(XmlNode StringRootNode)
            : base(StringRootNode)
        {
        }
    }
}
