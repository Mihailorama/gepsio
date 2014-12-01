﻿using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "presentationLink" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    public class PresentationLink
    {
        /// <summary>
        /// A list of locators used in this presentation link.
        /// </summary>
        public List<Locator> Locators
        {
            get;
            private set;
        }

        /// <summary>
        /// A list of presentation arcs used in this presentation link.
        /// </summary>
        public List<PresentationArc> PresentationArcs
        {
            get;
            private set;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal PresentationLink(INode PresentationLinkNode)
        {
            Locators = new List<Locator>();
            PresentationArcs = new List<PresentationArc>();
            foreach (INode CurrentChild in PresentationLinkNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("loc") == true)
                    Locators.Add(new Locator(CurrentChild));
                else if (CurrentChild.LocalName.Equals("presentationArc") == true)
                    PresentationArcs.Add(new PresentationArc(CurrentChild));
            }
        }
    }
}
