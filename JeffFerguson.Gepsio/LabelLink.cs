using System.Xml;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    public class LabelLink
    {
        private List<Locator> thisLocators;
        private List<LabelArc> thisLabelArcs;
        private List<Label> thisLabels;

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
        public List<LabelArc> LabelArcs
        {
            get
            {
                return thisLabelArcs;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public List<Label> Labels
        {
            get
            {
                return thisLabels;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal LabelLink(XmlNode LabelLinkNode)
        {
            thisLocators = new List<Locator>();
            thisLabelArcs = new List<LabelArc>();
            thisLabels = new List<Label>();
            ReadChildLocators(LabelLinkNode);
            ReadChildLabelArcs(LabelLinkNode);
            ReadChildLabels(LabelLinkNode);
            ResolveLocators();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ResolveLocators()
        {
            foreach (LabelArc CurrentLabelArc in thisLabelArcs)
                CurrentLabelArc.FromLocator = GetLocator(CurrentLabelArc.FromId);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildLabelArcs(XmlNode LabelLinkNode)
        {
            foreach (XmlNode CurrentChildNode in LabelLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("labelArc") == true)
                    thisLabelArcs.Add(new LabelArc(CurrentChildNode));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildLocators(XmlNode LabelLinkNode)
        {
            foreach (XmlNode CurrentChildNode in LabelLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("loc") == true)
                    thisLocators.Add(new Locator(CurrentChildNode));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildLabels(XmlNode LabelLinkNode)
        {
            foreach (XmlNode CurrentChildNode in LabelLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("label") == true)
                    thisLabels.Add(new Label(CurrentChildNode));
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
    }
}
