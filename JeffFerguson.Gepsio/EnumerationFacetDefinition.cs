using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    internal class EnumerationFacetDefinition : FacetDefinition
    {
        internal EnumerationFacetDefinition() : base("enumeration")
        {
            AddFacetPropertyDefinition(new FacetPropertyDefinition("value", typeof(String)));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("annotation", typeof(String), true));
        }
    }
}
