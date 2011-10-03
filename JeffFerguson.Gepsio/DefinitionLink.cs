using System.Xml;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    public class DefinitionLink
    {
        private List<Locator> thisLocators;
        private List<DefinitionArc> thisDefinitionArcs;
        private List<Locator> thisUnresolvedLocators;

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
        public List<DefinitionArc> DefinitionArcs
        {
            get
            {
                return thisDefinitionArcs;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal DefinitionLink(XmlNode DefinitionLinkNode)
        {
            thisLocators = new List<Locator>();
            thisDefinitionArcs = new List<DefinitionArc>();
            thisUnresolvedLocators = new List<Locator>();
            ReadChildLocators(DefinitionLinkNode);
            ReadChildDefinitionArcs(DefinitionLinkNode);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ResolveLocators(DefinitionArc CurrentDefinitionArc)
        {
            //CurrentDefinitionArc.FromLocator = GetUnresolvedLocator(CurrentDefinitionArc.FromId);
            CurrentDefinitionArc.FromLocator = GetLocator(CurrentDefinitionArc.FromId);
            if (CurrentDefinitionArc.FromLocator != null)
                thisUnresolvedLocators.Remove(CurrentDefinitionArc.FromLocator);

            //CurrentDefinitionArc.ToLocator = GetUnresolvedLocator(CurrentDefinitionArc.ToId);
            CurrentDefinitionArc.ToLocator = GetLocator(CurrentDefinitionArc.ToId);
            if (CurrentDefinitionArc.ToLocator != null)
                thisUnresolvedLocators.Remove(CurrentDefinitionArc.ToLocator);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildDefinitionArcs(XmlNode DefinitionLinkNode)
        {
            foreach (XmlNode CurrentChild in DefinitionLinkNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("definitionArc") == true)
                {
                    DefinitionArc NewDefinitionArc = new DefinitionArc(CurrentChild);
                    thisDefinitionArcs.Add(NewDefinitionArc);
                    ResolveLocators(NewDefinitionArc);
                }
            }
        }

        //------------------------------------------------------------------------------------
        // Read <loc> nodes and build a Locator object for each one.
        //
        // The implementation places the new Locator object in two places: one in a list of
        // all locators and one in a list of unresolved locators. Unresolved locators are
        // kept in that list until a <definitionArc> is found. Once a <definitionArc> is
        // processed, the locator references in the <definitionArc> are resolved using the
        // locators in the unresolved locator list. Once the resolution is performed, then
        // the Locator is removed from the unresolved locator list (but kept in the list of
        // all Locator objects).
        //
        // A more simplistic implementation would be to simply keep a single list of all
        // Locator objects and resolve all locator references in definition arcs once the
        // entire document is read. However, locator names are local to a <definitionLink>
        // and can be resued. Consider the following:
        //
        // <definitionLink xlink:type="extended" xlink:role="http://www.xbrl.org/2003/role/link">
        //      <loc xlink:type="locator" xlink:href="306-Required.xsd#flag" xlink:label="flag"/>
        //      <loc xlink:type="locator" xlink:href="306-Required.xsd#monetaryItem" xlink:label="monetaryItem"/>
        //      <definitionArc xlink:type="arc" xlink:from="flag" xlink:to="monetaryItem" xlink:arcrole="http://www.xbrl.org/2003/arcrole/requires-element"/>
        // </definitionLink>
        // <definitionLink xlink:type="extended" xlink:role="http://www.xbrl.org/2003/role/link">
        //      <loc xlink:type="locator" xlink:href="306-Required.xsd#tFlag" xlink:label="flag"/>
        //      <loc xlink:type="locator" xlink:href="306-Required.xsd#monetaryItem" xlink:label="monetaryItem"/>
        //      <definitionArc xlink:type="arc" xlink:from="flag" xlink:to="monetaryItem" xlink:arcrole="http://www.xbrl.org/2003/arcrole/requires-element"/>
        // </definitionLink>
        //
        // The locator label "flag" is used twice, for two different hrefs. Waiting until the
        // end to resolve all of the locator references into Locator objects may result in the
        // wrong locator being matched to a label. Therefore, the process is this:
        //
        // * read <loc> elements; place Locator into unresoilved list
        // * read <definitionArc>
        // * resolve "from" and "to" links to Locator objects found in unresolved list
        //
        // This also means that, after a <definitionArc> is fully processed, that the
        // unresolved Locator list is empty.
        //------------------------------------------------------------------------------------
        private void ReadChildLocators(XmlNode DefinitionLinkNode)
        {
            foreach (XmlNode CurrentChild in DefinitionLinkNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("loc") == true)
                {
                    Locator NewLocator = new Locator(CurrentChild);
                    thisLocators.Add(NewLocator);
                    thisUnresolvedLocators.Add(NewLocator);
                }
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private Locator GetUnresolvedLocator(string LocatorLabel)
        {
            foreach (Locator CurrentLocator in thisUnresolvedLocators)
            {
                if (CurrentLocator.Label.Equals(LocatorLabel) == true)
                    return CurrentLocator;
            }
            return null;
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
