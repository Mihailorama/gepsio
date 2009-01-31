using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public abstract class AnySimpleType : AnyType
    {
        private List<FacetDefinition> thisConstrainingFacetDefinitions;
        private List<Facet> thisFacets;

        public override bool NumericType
        {
            get
            {
                Type CurrentType = GetType();
                while (CurrentType != null)
                {
                    if (CurrentType.Equals(typeof(Decimal)) == true)
                        return true;
                    if (CurrentType.Equals(typeof(Double)) == true)
                        return true;
                    if (CurrentType.Equals(typeof(Float)) == true)
                        return true;
                    CurrentType = CurrentType.BaseType;
                }
                return false;
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

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal override decimal GetValueAfterApplyingPrecisionTruncation(int PrecisionValue)
        {
            if (this.NumericType == false)
                throw new NotSupportedException();
            decimal ValueAsDecimal = Convert.ToDecimal(this.ValueAsString);
            if (PrecisionValue > 0)
            {
                string WholePart;
                string TruncationAsString;

                int DecimalPointIndex = ValueAsString.IndexOf('.');
                if (DecimalPointIndex == -1)
                    WholePart = ValueAsString;
                else
                    WholePart = ValueAsString.Substring(0, DecimalPointIndex);
                if (PrecisionValue < WholePart.Length)
                    TruncationAsString = WholePart;
                else
                {
                    StringBuilder TruncationBuilder = new StringBuilder(WholePart.Substring(0, PrecisionValue));
                    for (int AddZeroCounter = 0; AddZeroCounter < (WholePart.Length - PrecisionValue); AddZeroCounter++)
                        TruncationBuilder.Append('0');
                    TruncationAsString = TruncationBuilder.ToString();
                }
                ValueAsDecimal = Convert.ToDecimal(TruncationAsString);
            }
            return ValueAsDecimal;
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal override decimal GetValueAfterApplyingDecimalsTruncation(int DecimalsValue)
        {
            if (this.NumericType == false)
                throw new NotSupportedException();
            decimal ValueAsDecimal = Convert.ToDecimal(this.ValueAsString);
            if (DecimalsValue > 0)
                ValueAsDecimal = Math.Round(ValueAsDecimal, DecimalsValue);
            return ValueAsDecimal;
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
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
    }
}
