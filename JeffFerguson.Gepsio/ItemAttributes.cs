using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    internal class ItemAttributes : FactAttributes
    {
        internal ItemAttributes() : base()
        {
            AddAttribute(new Attribute("contextRef", typeof(IDREF), true));
        }
    }
}
