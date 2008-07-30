using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class Facet
    {
        private FacetDefinition thisDefinition;
        private List<FacetProperty> thisProperties;

        internal List<FacetProperty> Properties
        {
            get
            {
                return thisProperties;
            }
        }

        internal static Facet CreateFacet(FacetDefinition Definition)
        {
            if (Definition is PatternFacetDefinition)
                return new PatternFacet(Definition);
            if (Definition is LengthFacetDefinition)
                return new LengthFacet(Definition);
            if (Definition is MaxLengthFacetDefinition)
                return new MaxLengthFacet(Definition);
            if (Definition is WhiteSpaceFacetDefinition)
                return new WhiteSpaceFacet(Definition);
            if (Definition is MinLengthFacetDefinition)
                return new WhiteSpaceFacet(Definition);
            if (Definition is EnumerationFacetDefinition)
                return new EnumerationFacet(Definition);
            string MessageFormat = AssemblyResources.GetName("FacetDefinitionNotSupportedForFacetCreation");
            StringBuilder MessageBuilder = new StringBuilder();
            MessageBuilder.AppendFormat(MessageFormat, Definition.GetType().ToString());
            throw new XbrlException(MessageBuilder.ToString());
        }

        protected Facet(FacetDefinition Definition)
        {
            thisDefinition = Definition;
            thisProperties = new List<FacetProperty>();
        }

        internal void AddFacetProperty(FacetPropertyDefinition Definition, string Value)
        {
            thisProperties.Add(new FacetProperty(Definition, Value));
        }

        internal FacetProperty GetFacetProperty(string PropertyName)
        {
            foreach (FacetProperty CurrentFacetProperty in thisProperties)
            {
                if (CurrentFacetProperty.Definition.Name.Equals(PropertyName) == true)
                    return CurrentFacetProperty;
            }
            return null;
        }
    }
}
