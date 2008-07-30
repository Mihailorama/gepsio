using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{   
    public class String : AnySimpleType<string>
    {
        protected override string ConvertStringValue()
        {
            return ValueAsString;
        }

        public String() : base()
        {
        }

        internal String(XmlNode StringRootNode) : base(StringRootNode)
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
