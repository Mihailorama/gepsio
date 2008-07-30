using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class NormalizedString : String
    {
        internal NormalizedString(XmlNode NormalizedStringRootNode) : base(NormalizedStringRootNode)
        {
        }

        protected override void AddConstrainingFacetDefinitions()
        {
            AddConstrainingFacetDefinition(new LengthFacetDefinition());
            AddConstrainingFacetDefinition(new MinLengthFacetDefinition());
            AddConstrainingFacetDefinition(new MaxLengthFacetDefinition());
            AddConstrainingFacetDefinition(new PatternFacetDefinition());
            AddConstrainingFacetDefinition(new EnumerationFacetDefinition());
            AddConstrainingFacetDefinition(new WhiteSpaceFacetDefinition());
        }
    }
}
