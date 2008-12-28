using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    internal class EssentialNumericItemAttributes : ItemAttributes
    {
        internal EssentialNumericItemAttributes() : base()
        {
            AddAttribute(new Attribute("unitRef", typeof(IDREF), true));
        }
    }
}
