using System.Xml;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    public class CalculationLink
    {
        private List<Locator> thisLocators;
        private List<CalculationArc> thisCalculationArcs;
        private List<SummationConcept> thisSummationConcepts;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public List<Locator> Locators
        {
            get
            {
                return thisLocators;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public List<CalculationArc> CalculationArcs
        {
            get
            {
                return thisCalculationArcs;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public List<SummationConcept> SummationConcepts
        {
            get
            {
                return thisSummationConcepts;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal CalculationLink(XmlNode CalculationLinkNode)
        {
            thisLocators = new List<Locator>();
            thisCalculationArcs = new List<CalculationArc>();
            thisSummationConcepts = new List<SummationConcept>();
            ReadChildLocators(CalculationLinkNode);
            ReadChildCalculationArcs(CalculationLinkNode);
            ResolveLocators();
            BuildSummationConcepts();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void BuildSummationConcepts()
        {
            foreach (CalculationArc CurrentCalculationArc in thisCalculationArcs)
                BuildSummationConcepts(CurrentCalculationArc);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void BuildSummationConcepts(CalculationArc CurrentCalculationArc)
        {
            SummationConcept CurrentSummationConcept;

            CurrentSummationConcept = FindSummationConcept(CurrentCalculationArc.FromLocator);
            if (CurrentSummationConcept == null)
            {
                CurrentSummationConcept = new SummationConcept(CurrentCalculationArc.FromLocator);
                thisSummationConcepts.Add(CurrentSummationConcept);
            }
            foreach(var CurrentToLocator in CurrentCalculationArc.ToLocators)
                CurrentSummationConcept.AddContributingConceptLocator(CurrentToLocator);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private SummationConcept FindSummationConcept(Locator SummationConceptLocator)
        {
            foreach (SummationConcept CurrentSummationConcept in thisSummationConcepts)
            {
                if (CurrentSummationConcept.SummationConceptLocator.Equals(SummationConceptLocator) == true)
                    return CurrentSummationConcept;
            }
            return null;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ResolveLocators()
        {
            foreach (CalculationArc CurrentCalculationArc in thisCalculationArcs)
                ResolveLocators(CurrentCalculationArc);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ResolveLocators(CalculationArc CurrentCalculationArc)
        {
            CurrentCalculationArc.FromLocator = GetLocator(CurrentCalculationArc.FromId);
            foreach (Locator CurrentLocator in thisLocators)
            {
                if (CurrentLocator.Label.Equals(CurrentCalculationArc.ToId) == true)
                    CurrentCalculationArc.AddToLocator(CurrentLocator);
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildCalculationArcs(XmlNode CalculationLinkNode)
        {
            foreach (XmlNode CurrentChildNode in CalculationLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("calculationArc") == true)
                    thisCalculationArcs.Add(new CalculationArc(CurrentChildNode));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildLocators(XmlNode CalculationLinkNode)
        {
            foreach (XmlNode CurrentChildNode in CalculationLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("loc") == true)
                    thisLocators.Add(new Locator(CurrentChildNode));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private Locator GetLocator(string LocatorLabel)
        {
            foreach (Locator CurrentLocator in thisLocators)
            {
                if (CurrentLocator.Label.Equals(LocatorLabel) == true)
                    return CurrentLocator;
            }
            return null;
        }

        /// <summary>
        /// Find the calculation arc that is referenced by the given locator.
        /// </summary>
        /// <remarks>
        /// The "to" link is searched.
        /// </remarks>
        /// <param name="SourceLocator">The locator used as the source of the search.</param>
        /// <returns>The CalculationArc referenced by the Locator, or null if a calculation arc cannot be found.</returns>
        internal CalculationArc GetCalculationArc(Locator SourceLocator)
        {
            foreach (CalculationArc CurrentCalculationArc in CalculationArcs)
            {
                if (CurrentCalculationArc.ToId.Equals(SourceLocator.Label) == true)
                    return CurrentCalculationArc;
            }
            return null;
        }
    }
}
