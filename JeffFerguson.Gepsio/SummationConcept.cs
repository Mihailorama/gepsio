using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    public class SummationConcept
    {
        private Locator thisSummationConceptLocator;
        private List<Locator> thisContributingConceptLocators;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public Locator SummationConceptLocator
        {
            get
            {
                return thisSummationConceptLocator;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public List<Locator> ContributingConceptLocators
        {
            get
            {
                return thisContributingConceptLocators;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal SummationConcept(Locator SummationConceptLocator)
        {
            thisSummationConceptLocator = SummationConceptLocator;
            thisContributingConceptLocators = new List<Locator>();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal void AddContributingConceptLocator(Locator ContributingConceptLocator)
        {
            if(thisContributingConceptLocators.Contains(ContributingConceptLocator) == false)
                thisContributingConceptLocators.Add(ContributingConceptLocator);
        }
    }
}
