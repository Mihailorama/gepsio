using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Integer : Decimal
    {
        internal Integer(XmlNode StringRootNode) : base(StringRootNode)
        {
        }
    }
}
