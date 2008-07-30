using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class FacetDefinition
    {
        private string thisName;
        private List<FacetPropertyDefinition> thisFacetPropertyDefinitions;

        internal string Name
        {
            get
            {
                return thisName;
            }
        }

        internal List<FacetPropertyDefinition> PropertyDefinitions
        {
            get
            {
                return thisFacetPropertyDefinitions;
            }
        }

        internal FacetDefinition(string FacetName)
        {
            thisName = FacetName;
            thisFacetPropertyDefinitions = new List<FacetPropertyDefinition>();
        }

        internal void AddFacetPropertyDefinition(FacetPropertyDefinition NewFacetPropertyDefinition)
        {
            thisFacetPropertyDefinitions.Add(NewFacetPropertyDefinition);
        }
    }
}
