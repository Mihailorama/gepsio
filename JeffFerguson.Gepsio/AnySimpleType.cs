using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public abstract class AnySimpleType<T> : AnyType
    {
        private string thisValueAsString;
        private T thisValue;
        private List<FacetDefinition> thisConstrainingFacetDefinitions;
        private List<Facet> thisFacets;

        public override string ValueAsString
        {
            get
            {
                return thisValueAsString;
            }
            set
            {
                thisValueAsString = value;
                this.Value = ConvertStringValue();
            }
        }

        public T Value
        {
            get
            {
                return thisValue;
            }
            protected set
            {
                thisValue = value;
            }
        }

        public List<Facet> Facets
        {
            get
            {
                return thisFacets;
            }
        }

        internal AnySimpleType()
        {
            thisConstrainingFacetDefinitions = new List<FacetDefinition>();
            thisFacets = new List<Facet>();
            AddConstrainingFacetDefinitions();
        }

        internal AnySimpleType(XmlNode TypeRootNode) : this()
        {
            if (TypeRootNode != null)
            {
                foreach (XmlNode ChildNode in TypeRootNode.ChildNodes)
                    AddFacet(ChildNode);
            }
        }

        internal List<Facet> GetFacets(Type FacetType)
        {
            List<Facet> NewList;

            NewList = new List<Facet>();
            foreach (Facet CurrentFacet in thisFacets)
            {
                if (CurrentFacet.GetType() == FacetType)
                    NewList.Add(CurrentFacet);
            }
            return NewList;
        }

        private void AddFacet(XmlNode FacetNode)
        {
            ValidateFacet(FacetNode);
        }

        private void ValidateFacet(XmlNode FacetNode)
        {
            foreach (FacetDefinition CurrentFacetDefinition in thisConstrainingFacetDefinitions)
            {
                if (CurrentFacetDefinition.Name.Equals(FacetNode.Name) == true)
                {
                    ProcessFacet(CurrentFacetDefinition, FacetNode);
                    return;
                }
            }
            string MessageFormat = AssemblyResources.GetName("UnsupportedFacet");
            StringBuilder MessageBuilder = new StringBuilder();
            MessageBuilder.AppendFormat(MessageFormat, FacetNode.Name, this.GetType().Name);
            throw new XbrlException(MessageBuilder.ToString());
        }

        private void ProcessFacet(FacetDefinition CurrentFacetDefinition, XmlNode FacetNode)
        {
            Facet NewFacet;

            NewFacet = Facet.CreateFacet(CurrentFacetDefinition);
            foreach (XmlAttribute CurrentAttribute in FacetNode.Attributes)
            {
                foreach (FacetPropertyDefinition CurrentPropertyDefinition in CurrentFacetDefinition.PropertyDefinitions)
                {
                    if (CurrentAttribute.Name.Equals(CurrentPropertyDefinition.Name) == true)
                    {
                        NewFacet.AddFacetProperty(CurrentPropertyDefinition, CurrentAttribute.Value);
                        thisFacets.Add(NewFacet);
                        return;
                    }
                }
                string MessageFormat = AssemblyResources.GetName("UnsupportedFacetProperty");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, CurrentAttribute.Name, CurrentFacetDefinition.Name);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        protected void AddConstrainingFacetDefinition(FacetDefinition ConstrainingFacet)
        {
            thisConstrainingFacetDefinitions.Add(ConstrainingFacet);
        }

        protected virtual void AddConstrainingFacetDefinitions()
        {
        }

        protected abstract T ConvertStringValue();
    }
}
