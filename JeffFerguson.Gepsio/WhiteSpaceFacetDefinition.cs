using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    internal class WhiteSpaceFacetDefinition : FacetDefinition
    {
        internal WhiteSpaceFacetDefinition() : base("whiteSpace")
        {
            AddFacetPropertyDefinition(new FacetPropertyDefinition("value", typeof(String)));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("fixed", typeof(Boolean), "false"));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("annotation", typeof(String), true));
        }
    }
}
