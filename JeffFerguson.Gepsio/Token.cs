using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Token : NormalizedString
    {
        private List<string> thisEnumerationValues;

        public List<string> EnumerationValues
        {
            get
            {
                CollectEnumerationValues();
                return thisEnumerationValues;
            }
        }

        internal Token(XmlNode TokenRootNode) : base(TokenRootNode)
        {
            thisEnumerationValues = null;
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

        private void CollectEnumerationValues()
        {
            if (thisEnumerationValues != null)
                return;
            thisEnumerationValues = new List<string>();
            List<Facet> EnumerationFacets = GetFacets(typeof(EnumerationFacet));
            foreach (Facet EnumerationFacet in EnumerationFacets)
            {
                FacetProperty ValueProperty = EnumerationFacet.GetFacetProperty("value");
                if (ValueProperty != null)
                {
                    String StringValueProperty = ValueProperty.Value as String;
                    thisEnumerationValues.Add(StringValueProperty.Value);
                }
            }
        }
    }
}
