using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class FootnoteLink
    {
        private XmlNode thisFootnoteLinkNode;
        private List<FootnoteLocator> thisFootnoteLocators;
        private List<Footnote> thisFootnotes;
        private List<FootnoteArc> thisFootnoteArcs;

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<FootnoteArc> FootnoteArcs
        {
            get
            {
                return thisFootnoteArcs;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<FootnoteLocator> FootnoteLocators
        {
            get
            {
                return thisFootnoteLocators;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal FootnoteLink(XmlNode FootnoteLinkNode)
        {
            thisFootnoteLinkNode = FootnoteLinkNode;
            thisFootnotes = new List<Footnote>();
            thisFootnoteLocators = new List<FootnoteLocator>();
            thisFootnoteArcs = new List<FootnoteArc>();
            foreach (XmlNode ChildNode in thisFootnoteLinkNode.ChildNodes)
            {
                if (ChildNode.LocalName.Equals("loc") == true)
                    thisFootnoteLocators.Add(new FootnoteLocator(this, ChildNode));
                else if (ChildNode.LocalName.Equals("footnote") == true)
                    thisFootnotes.Add(new Footnote(this, ChildNode));
                else if (ChildNode.LocalName.Equals("footnoteArc") == true)
                    thisFootnoteArcs.Add(new FootnoteArc(this, ChildNode));
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public FootnoteLocator GetLocator(string Label)
        {
            foreach (FootnoteLocator CurrentLocator in thisFootnoteLocators)
            {
                if (CurrentLocator.Label.Equals(Label) == true)
                    return CurrentLocator;
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public Footnote GetFootnote(string Label)
        {
            foreach (Footnote CurrentFootnote in thisFootnotes)
            {
                if (CurrentFootnote.Label.Equals(Label) == true)
                    return CurrentFootnote;
            }
            return null;
        }
    }
}
